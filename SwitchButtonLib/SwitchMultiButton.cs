// Description  A smart switchable button,support multiple tabs.
// Ported to Xamarin.Android by Rofiq Setiawan (rofiqsetiawan@gmail.com)
//
// Create Time  2016/7/27 14:57
// Author   KingJA
// Email    kingjavip@gmail.com

using System;
using Android.OS;
using Android.Content;
using Android.Graphics;
using Android.Text;
using Android.Util;
using Android.Views;
using R = Lib.KingJA.SwitchButton.Resource;

namespace Lib.KingJA.SwitchButton
{
	/// <summary>
	/// A smart switchable button, support multiple tabs.
	/// </summary>
	public class SwitchMultiButton : View, SwitchMultiButton.IOnSwitchListener
	{
		private const string MyTag = nameof(SwitchMultiButton);

		/*default value*/
		private static string[] _tabTexts = { "L", "R" };
		private int _tabNum = _tabTexts.Length;
		private const float StrokeRadius = 0;
		private const float StrokeWidth = 2;
		private const float TextSize = 14;
		private const int SelectedColor = unchecked((int)0xffeb7b00);
		private const int SelectedTab = 0;

		/*other*/
		private Paint _strokePaint;
		private Paint _fillPaint;
		private int _width;
		private int _height;
		private TextPaint _selectedTextPaint;
		private TextPaint _unselectedTextPaint;
		private IOnSwitchListener _onSwitchListener;
		private float _strokeRadius;
		private float _strokeWidth;
		private int _selectedColor;
		private float _textSize;
		private int _selectedTab;
		private float _perWidth;
		private float _textHeightOffset;
		private Paint.FontMetrics _fontMetrics;

		#region Constructors

		public SwitchMultiButton(Context context) : this(context, null)
		{
		}

		public SwitchMultiButton(Context context, IAttributeSet attrs) : this(context, attrs, 0)
		{
		}

		public SwitchMultiButton(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
		{
			InitAttrs(context, attrs);
			InitPaint();
		}

		#endregion



		/// <summary>
		/// Get the values of attributes
		/// </summary>
		/// <param name="context">Context.</param>
		/// <param name="attrs">Attrs.</param>
		private void InitAttrs(Context context, IAttributeSet attrs)
		{
			var typedArray = context.ObtainStyledAttributes(attrs, R.Styleable.SwitchMultiButton);
			_strokeRadius = typedArray.GetDimension(R.Styleable.SwitchMultiButton_strokeRadius, StrokeRadius);
			_strokeWidth = typedArray.GetDimension(R.Styleable.SwitchMultiButton_strokeWidth, StrokeWidth);
			_textSize = typedArray.GetDimension(R.Styleable.SwitchMultiButton_textSize, TextSize);
			_selectedColor = typedArray.GetColor(R.Styleable.SwitchMultiButton_selectedColor, SelectedColor);
			_selectedTab = typedArray.GetInteger(R.Styleable.SwitchMultiButton_selectedTab, SelectedTab);

			var switchTabsResId = typedArray.GetResourceId(R.Styleable.SwitchMultiButton_switchTabs, 0);
			if (switchTabsResId != 0)
			{
				_tabTexts = Resources.GetStringArray(switchTabsResId);
			}
			typedArray.Recycle();
		}

		/// <summary>Define paints.</summary>
		private void InitPaint()
		{
			// Round rectangle paint
			_strokePaint = new Paint
			{
				Color = _selectedColor.ToColor(),
				AntiAlias = true,
				StrokeWidth = _strokeWidth
			};
			_strokePaint.SetStyle(Paint.Style.Stroke);


			// Selected paint
			_fillPaint = new Paint
			{
				Color = _selectedColor.ToColor()
			};
			_strokePaint.AntiAlias = true;
			_fillPaint.SetStyle(Paint.Style.FillAndStroke);


			// Selected text paint
			_selectedTextPaint = new TextPaint(PaintFlags.AntiAlias)
			{
				TextSize = _textSize,
				Color = unchecked((int)0xffffffff).ToColor()
			};
			_strokePaint.AntiAlias = true;

			// Unselected text paint
			_unselectedTextPaint = new TextPaint(PaintFlags.AntiAlias)
			{
				TextSize = _textSize,
				Color = _selectedColor.ToColor()
			};

			_strokePaint.AntiAlias = true;
			_textHeightOffset = -(_selectedTextPaint.Ascent() + _selectedTextPaint.Descent()) * 0.5f;
			_fontMetrics = _selectedTextPaint.GetFontMetrics();
		}


		protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
		{
			SetMeasuredDimension(
				GetExpectSize(DefaultWidth, widthMeasureSpec),
				GetExpectSize(DefaultHeight, heightMeasureSpec)
			);
		}

		/// <summary>
		/// Get default height when android:layout_height="wrap_content"
		/// </summary>
		/// <returns>The default height.</returns>
		private int DefaultHeight
			=> (int)(_fontMetrics.Bottom - _fontMetrics.Top) + PaddingTop + PaddingBottom;

		/// <summary>
		/// Get default width when android:layout_width="wrap_content"
		/// </summary>
		/// <returns>The default width.</returns>
		private int DefaultWidth
		{
			get
			{
				var tabTextWidth = 0f;
				var tabs = _tabTexts.Length;
				for (var i = 0; i < tabs; i++)
				{
					tabTextWidth = Math.Max(tabTextWidth, _selectedTextPaint.MeasureText(_tabTexts[i]));
				}
				var totalTextWidth = tabTextWidth * tabs;
				var totalStrokeWidth = _strokeWidth * tabs;
				var totalPadding = (PaddingRight + PaddingLeft) * tabs;
				return (int)(totalTextWidth + totalStrokeWidth + totalPadding);
			}
		}


		/// <summary>
		/// Get expect size.
		/// </summary>
		/// <returns>The expect size.</returns>
		/// <param name="size">Size.</param>
		/// <param name="measureSpec">Measure spec.</param>
		private static int GetExpectSize(int size, int measureSpec)
		{
			var result = size;
			var specMode = MeasureSpec.GetMode(measureSpec);
			var specSize = MeasureSpec.GetSize(measureSpec);
			switch (specMode)
			{
				case MeasureSpecMode.Exactly:
					result = specSize;
					break;

				case MeasureSpecMode.Unspecified:
					result = size;
					break;

				case MeasureSpecMode.AtMost:
					result = Math.Min(size, specSize);
					break;
			}
			return result;
		}

		protected override void OnDraw(Canvas canvas)
		{
			base.OnDraw(canvas);

			var left = _strokeWidth * 0.5f;
			var top = _strokeWidth * 0.5f;
			var right = _width - _strokeWidth * 0.5f;
			var bottom = _height - _strokeWidth * 0.5f;

			// Draw rounded rectangle
			canvas.DrawRoundRect(
				new RectF(left, top, right, bottom),
				_strokeRadius,
				_strokeRadius,
				_strokePaint
			);

			// Draw line
			for (var i = 0; i < (_tabNum - 1); i++)
			{
				canvas.DrawLine(
					_perWidth * (i + 1),
					top,
					_perWidth * (i + 1),
					bottom,
					_strokePaint
				);
			}

			// Draw tab and line
			for (var i = 0; i < _tabNum; i++)
			{
				var tabText = _tabTexts[i];
				var tabTextWidth = _selectedTextPaint.MeasureText(tabText);
				if (i == _selectedTab)
				{
					// Draw selected tab
					if (i == 0)
					{
						DrawLeftPath(canvas, left, top, bottom);
					}
					else if (i == (_tabNum - 1))
					{
						DrawRightPath(canvas, top, right, bottom);
					}
					else
					{
						canvas.DrawRect(new RectF(_perWidth * i, top, _perWidth * (i + 1), bottom), _fillPaint);
					}

					// Draw selected text
					canvas.DrawText(
						tabText,
						0.5f * _perWidth * (2 * i + 1) - 0.5f * tabTextWidth,
						_height * 0.5f + _textHeightOffset,
						_selectedTextPaint
					);
				}
				else
				{
					// Draw unselected text
					canvas.DrawText(
						tabText,
						0.5f * _perWidth * (2 * i + 1) - 0.5f * tabTextWidth,
						_height * 0.5f + _textHeightOffset,
						_unselectedTextPaint
					);
				}
			}
		}


		/// <summary>
		/// Draws the left path.
		/// </summary>
		/// <param name="canvas">Canvas.</param>
		/// <param name="left">Left.</param>
		/// <param name="top">Top.</param>
		/// <param name="bottom">Bottom.</param>
		private void DrawLeftPath(Canvas canvas, float left, float top, float bottom)
		{
			var leftPath = new Path();
			leftPath.MoveTo(left + _strokeRadius, top);
			leftPath.LineTo(_perWidth, top);
			leftPath.LineTo(_perWidth, bottom);
			leftPath.LineTo(left + _strokeRadius, bottom);
			leftPath.ArcTo(new RectF(left, bottom - 2 * _strokeRadius, left + 2 * _strokeRadius, bottom), 90, 90);
			leftPath.LineTo(left, top + _strokeRadius);
			leftPath.ArcTo(new RectF(left, top, left + 2 * _strokeRadius, top + 2 * _strokeRadius), 180, 90);
			canvas.DrawPath(leftPath, _fillPaint);
		}


		/// <summary>
		/// Draw right path.
		/// </summary>
		/// <param name="canvas">Canvas.</param>
		/// <param name="top">Top.</param>
		/// <param name="right">Right.</param>
		/// <param name="bottom">Bottom.</param>
		private void DrawRightPath(Canvas canvas, float top, float right, float bottom)
		{
			var rightPath = new Path();
			rightPath.MoveTo(right - _strokeRadius, top);
			rightPath.LineTo(right - _perWidth, top);
			rightPath.LineTo(right - _perWidth, bottom);
			rightPath.LineTo(right - _strokeRadius, bottom);
			rightPath.ArcTo(new RectF(right - 2 * _strokeRadius, bottom - 2 * _strokeRadius, right, bottom), 90, -90);
			rightPath.LineTo(right, top + _strokeRadius);
			rightPath.ArcTo(new RectF(right - 2 * _strokeRadius, top, right, top + 2 * _strokeRadius), 0, -90);
			canvas.DrawPath(rightPath, _fillPaint);
		}

		protected override void OnSizeChanged(int w, int h, int oldw, int oldh)
		{
			base.OnSizeChanged(w, h, oldw, oldh);
			_width = MeasuredWidth;
			_height = MeasuredHeight;
			_perWidth = _width / (float)_tabNum;
			CheckAttrs();
		}

		/// <summary>
		/// check attribute whehere suitable.
		/// </summary>
		private void CheckAttrs()
		{
			if (_strokeRadius > 0.5f * _height)
			{
				_strokeRadius = 0.5f * _height;
			}
		}

		public override bool OnTouchEvent(MotionEvent e)
		{
			if (e.Action == MotionEventActions.Up)
			{
				var x = e.GetX();

				for (var i = 0; i < _tabNum; i++)
				{
					if (x > _perWidth * i && x < _perWidth * (i + 1))
					{
						if (_selectedTab == i)
						{
							return true;
						}
						_selectedTab = i;

						// Set Listener
						_onSwitchListener?.OnSwitch(i, _tabTexts[i]);

						// Event Handler
						OnSwitch(i, _tabTexts[i]);
					}
				}
				Invalidate();
			}
			return true;
		}





		#region Interface and Listener

		/// <summary>
		/// Called when switched.
		/// </summary>
		public interface IOnSwitchListener
		{
			void OnSwitch(int position, string tabText);
		}

		public SwitchMultiButton SetOnSwitchListener(IOnSwitchListener onSwitchListener)
		{
			_onSwitchListener = onSwitchListener;
			return this;
		}

		public class OnSwitchListener : IOnSwitchListener
		{
			private readonly Action<int, string> _callback;

			public OnSwitchListener(Action<int, string> callback)
			{
				_callback = callback;
			}

			public void OnSwitch(int position, string tabText)
			{
				_callback(position, tabText);
			}
		}

		public class OnSwitchEventArgs : EventArgs
		{
			public int Position { get; set; }
			public string TabText { get; set; }
		}

		/// <summary>
		/// Occurs when switched.
		/// </summary>
		public event EventHandler<OnSwitchEventArgs> Switch;

		public void OnSwitch(int position, string tabText)
		{
			Switch?.Invoke(
				this,
				new OnSwitchEventArgs
				{
					Position = position,
					TabText = tabText
				}
			);
		}

		#endregion



		#region Set and Get

		/// <summary>
		/// Get position of selected tab.
		/// </summary>
		/// <returns>The selected tab.</returns>
		public int GetSelectedTab() => _selectedTab;


		/// <summary>
		/// Sets the selected tab.
		/// </summary>
		/// <returns>The selected tab.</returns>
		/// <param name="selectedTab">Selected tab.</param>
		public SwitchMultiButton SetSelectedTab(int selectedTab)
		{
			_selectedTab = selectedTab;
			Invalidate();

			// Set Listener
			_onSwitchListener?.OnSwitch(selectedTab, _tabTexts[selectedTab]);

			// Event Handler
			OnSwitch(selectedTab, _tabTexts[selectedTab]);

			return this;
		}


		/// <summary>
		/// Set data for the switchbutton.
		/// </summary>
		/// <returns>The text.</returns>
		/// <param name="tagTexts">Tag texts.</param>
		public SwitchMultiButton SetText(params string[] tagTexts)
		{
#if DEBUG
			Log.Debug("Input", $"Input {tagTexts.Length} text(s)");
#endif
			if (tagTexts.Length > 1)
			{
				_tabTexts = tagTexts;
				_tabNum = tagTexts.Length;
				RequestLayout();
				return this;
			}

			throw new ArgumentException($"The size of {nameof(tagTexts)} should greater then 1");
		}

		#endregion



		#region Save and restore

		protected override IParcelable OnSaveInstanceState()
		{
			var bundle = new Bundle();
			bundle.PutParcelable("View", base.OnSaveInstanceState());
			bundle.PutFloat("StrokeRadius", _strokeRadius);
			bundle.PutFloat("StrokeWidth", _strokeWidth);
			bundle.PutFloat("TextSize", _textSize);
			bundle.PutInt("SelectedColor", _selectedColor);
			bundle.PutInt("SelectedTab", _selectedTab);
			return bundle;
		}

		protected override void OnRestoreInstanceState(IParcelable state)
		{
			if (state is Bundle)
			{
				var bundle = (Bundle)state;
				_strokeRadius = bundle.GetFloat("StrokeRadius");
				_strokeWidth = bundle.GetFloat("StrokeWidth");
				_textSize = bundle.GetFloat("TextSize");
				_selectedColor = bundle.GetInt("SelectedColor");
				_selectedTab = bundle.GetInt("SelectedTab");
				base.OnRestoreInstanceState((IParcelable)bundle.GetParcelable("View"));
			}
			else
			{
				base.OnRestoreInstanceState(state);
			}
		}

		#endregion


	}
}

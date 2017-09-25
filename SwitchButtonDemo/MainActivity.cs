/*
 *  Copyright (C) 2016 KingJA
 *
 *  Licensed under the Apache License, Version 2.0 (the "License");
 *  you may not use this file except in compliance with the License.
 *  You may obtain a copy of the License at
 *
 *  http://www.apache.org/licenses/LICENSE-2.0
 *
 *  Unless required by applicable law or agreed to in writing, software
 *  distributed under the License is distributed on an "AS IS" BASIS,
 *  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 *  See the License for the specific language governing permissions and
 *  limitations under the License.
 *
 */

/**
 * Description: A smart switchable button,support multiple tabs. Ported to Xamarin.Android by Rofiq Setiawan (rofiqsetiawan@gmail.com)
 * Create Time: 2016/7/27 10:26
 * Author:KingJA
 * Email:kingjavip@gmail.com
 * update:add into Jenkins 16:23
 */

using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Util;
using Android.Widget;
using Lib.KingJA.SwitchButton;
using R = SwitchButtonDemo.Resource;

namespace SwitchButtonDemo
{
	[Activity(Label = "SwitchButtonDemo", MainLauncher = true, Icon = "@mipmap/ic_launcher", Theme = "@style/AppTheme")]
	public class MainActivity : AppCompatActivity
	{
		private readonly string[] _tabTexts1 = { "Wizards 1", "Handsome guy", "Big wet", "Fierce brother" };
		private readonly string[] _tabTexts4 = { "Already", "At home", "Wait for you" };


		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			SetContentView(R.Layout.activity_main);


			var switch1 = FindViewById<SwitchMultiButton>(R.Id.switchmultibutton1);

			// With Set Listener
			switch1.SetText("One", "Two", "Three", "Four")
				   .SetOnSwitchListener(OnSwitchListener);

			// With Event Handler
			switch1.Switch += (s, e) =>
			{
				Log.Warn("Hello from EventHandler", $"Click on {e.TabText} at position: {e.Position}");
			};


			// Don't enable all SwitchMultiButton at once
			//FindViewById<SwitchMultiButton>(R.Id.switchmultibutton2).SetText("Star Star", "Cruel refused")
			//														.SetOnSwitchListener(OnSwitchListener);

			//FindViewById<SwitchMultiButton>(R.Id.switchmultibutton3).SetText("One", "Two")
			//														.SetOnSwitchListener(OnSwitchListener)
			//														.SetSelectedTab(1);

			//FindViewById<SwitchMultiButton>(R.Id.switchmultibutton4).SetText(_tabTexts4)
			//														.SetOnSwitchListener(OnSwitchListener);




		}

		private SwitchMultiButton.OnSwitchListener OnSwitchListener => new SwitchMultiButton.OnSwitchListener((position, tabText) =>
		{
			Toast.MakeText(this, tabText, ToastLength.Short).Show();
		});
	}
}


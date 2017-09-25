# SwitchButton
A smart switchable button,support multiple tabs.

This is Xamarin port of [KingJA's SwitchButton](https://github.com/KingJA/SwitchButton).

## Preview
![](https://github.com/KingJA/SwitchButton/raw/master/img/usage.gif)

## Custom Attribute
| attribute | format | example  |
| :------------- |:-------------| :-----|
| strokeRadius | dimension      | app:strokeRadius="5dp" |
| strokeWidth | dimension      | app:strokeWidth="2dp" |
| textSize | dimension      | app:textSize="16sp" |
| selectedColor | color/reference     | app:selectedColor="@color/red" |
| selectedTab | integer     | app:selectedTab="1" |
| switchTabs | reference     | app:switchTabs="@array/switch_tabs" |

## Installation from Nuget
```
PM> Install-Package Karamunting.Android.KingJA.SwitchButton -Version 1.1.3
```

## Usage
### step 1
```xml
<lib.kingja.switchbutton.SwitchMultiButton
  android:layout_width="wrap_content"
  android:layout_height="wrap_content"
  android:padding="8dp"
  app:strokeRadius="5dp"
  app:strokeWidth="1dp"
  app:selectedTab="0"
  app:selectedColor="#eb7b00"
  app:switchTabs="@array/switch_tabs"
  app:textSize="14sp" />
```

### step 2
```csharp
protected override void OnCreate(Bundle savedInstanceState)
{
  // Init
  var switch1 = FindViewById<SwitchMultiButton>(R.Id.switchmultibutton1);

  // With Set Listener
  switch1.SetText("One", "Two", "Three", "Four")
         .SetOnSwitchListener(OnSwitchListener);
}

private SwitchMultiButton.OnSwitchListener OnSwitchListener => new SwitchMultiButton.OnSwitchListener((position, tabText) =>
{
  Toast.MakeText(this, tabText, ToastLength.Short).Show();
});
```

or just use `.Switch` Event Handler for simplicity

```csharp
// With Event Handler
switch1.Switch += (s, e) =>
{
  Log.Warn("Hello from EventHandler", $"Click on {e.TabText} at position: {e.Position}");
};
```

## License
```
Copyright 2017 KingJA

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
```
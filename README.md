# WPF-JS-Angular Messaging System Demo
## Contents
This repo demonstrates how an Angular child app can communicate its state to eighter a Javascript parent app or a WPF parent app. 
The Repository contains three Projects:
- messaging-system-child: An Angular App child component containing:
  - a list to determine state which is supposed to be communicated to the respective parent component
  - a button to trigger state communication to the parent component
  - a log displaying events
- messaging-system-parent-web (see [git submodule](https://github.com/beyerl/messaging-system-parent-web)): A Javascript parent component which hosts the messaging-system-child inside an iframe. The host app contains:
  - a button which triggers the child app to communicate its current state to the parent
  - a log displaying events
- messaging-system-parent-wpf (see ([git submodule](https://github.com/beyerl/messaging-system-parent-web)): A WPF parent component which hosts the messaging-system-child inside a Webview2 control. The host app contains:
  - a button which triggers the child app to communicate its current state to the parent
  - a log displaying events

## Getting Started
### messaging-system-parent-wpf
1. Check out with

```
git clone --recurse-submodules
```

2. Open solution MessagingSystem.sln in Visual Studio
3. Build and run

### messaging-system-parent-web
1. Open [https://beyerl.github.io/messaging-system-parent-web/](https://beyerl.github.io/messaging-system-parent-web/)

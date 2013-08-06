This is the Visual Studio 2010 version, using the .NET Framework 4.0
James Hurst


Some random notes:

// Notes on using this:
//
// To have the virtual keyboard (VK) remember whether it was open for your Window when it last closed,
// and automatically open with your Window when you next open it:
//
// Add an entry within your application's Settings to store this setting, in UserScope and of type bool.
// In this example, the name of the entry is "IsVirtualKeyboardUpForItemEditDialog".
//
// Handle the ContentRendered event thus:
//
//   void OnContentRendered(object sender, EventArgs e)
//   {
//       if (Properties.Settings.Default.IsVirtualKeyboardUpForItemEditDialog)
//       {
//           VirtualKeyboard.ShowOrAttachTo(this, ref _virtualKeyboard);
//       }
//   }
//
//
// Handle the Closing event thus:
//
//   void OnClosing(object sender, System.ComponentModel.CancelEventArgs e)
//   {
//       // If the virtual keyboard was up, then remember that so we can bring it up next time this same dialog-window is opened.
//       bool virtualKeyboardIsUp = _virtualKeyboard != null && _virtualKeyboard.IsUp;
//       Properties.Settings.Default.IsVirtualKeyboardUpForItemEditDialog = virtualKeyboardIsUp;
//       Properties.Settings.Default.Save();
//   }
//
// That needs to be in Closing, not Closed, because it needs to happen before the VK itself closes,
// which it automatically does if your Window is opened as a modal dialog.

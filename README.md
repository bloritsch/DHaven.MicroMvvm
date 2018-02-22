# DHaven.MicroMvvm

[![Join the chat at https://gitter.im/DHaven-MicroMvvm/Lobby](https://badges.gitter.im/DHaven-MicroMvvm/Lobby.svg)](https://gitter.im/DHaven-MicroMvvm/Lobby?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge&utm_content=badge)

A very simple ViewModel first MVVM infrastructure that works equally well with WPF (requires MahApps.Metro) and UWP apps.
Does not require a container or component framework, and does not prevent using one either.

A key feature is the `IWindowManager` to open new windows, display notifications, show dialogs.  The API is consistent for both UWP and WPF applications so you can hit the ground running with an API that is familiar.

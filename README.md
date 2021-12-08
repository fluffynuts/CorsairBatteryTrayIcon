# Corsair Battery Tray Icon

## What is it?
A fork of https://github.com/mx0c/Corsair-Headset-Battery-Overlay aimed at providing a simple systray
battery status icon for Corsair wireless headphones.

## Why fork?
The hard work of figuring out headphone state is already done there - but the original intent of the
project is to provide an overlay that will show over apps and games, where I'd prefer an unobtrusive
systray icon. I've already submitted a PR to facilitate what I want from the project, but I also now
want to lighten it up, removing the WPF dependency in favor of good ol' winforms for handling the
icon, and remove the overlay altogether, unless someone _really_ wants it back, in which case, I'll
figure out a winforms way to do it (:

## Installing
Either build from source or download a release, copy all binaries somewhere and run the .exe

## Building
- build with Visual Studio or Rider
- build with VS Build Tools & npm scripts
    - install VS Build Tools
    - `npm ci`
    - `npm run build`

## Testing
As for building with npm, above, but `npm test`

## Current status
[x] tray icon percentage and icons work
[x] hard-coded to support the HS70 Pro for now
[ ] support multiple devices via config file
[ ] support device selection via the tray menu
[ ] support persisting device selection
[ ] add estimated remaining time to tooltip
[ ] add an installer

# AppleCaps

AppleCaps is a utility designed to harmonize keyboard layout switching between macOS and Windows environments, particularly for users who frequently alternate between these operating systems and require a consistent keyboard shortcut for changing keyboard layouts. This utility specifically addresses the challenge of using different keyboard shortcuts on macOS and Windows to switch keyboard layouts. AppleCaps emulates macOS behavior on Windows by intercepting Caps Lock key presses and translating them into Ctrl-Shift key presses, enabling a seamless and intuitive keyboard layout switching experience.

## Features

- **Keyboard Layout Switching**: Mimics the macOS keyboard layout switching shortcut (Caps Lock) on Windows by translating it into the Windows equivalent (Ctrl-Shift).
- **Caps Lock Functionality**: Retains the original functionality of the Caps Lock key through a long press, similar to the behavior observed in macOS.
- **System Tray Icon**: Runs discreetly with an icon in the Windows taskbar tray for easy access and management.

## Getting Started

### Prerequisites

- Windows operating system
- .NET Framework 8.0
- C# and Windows Forms compatible development environment (for building from source)

### Building from Source

To build AppleCaps from source (the only sane method for a program that intercepts your keystrokes) follow these steps:

1. Clone the repository:
   ```
   git clone https://github.com/roman-usatenko/AppleCaps.git
   ```
2. Take a look at the sources
3. Install .NET SDK
4. Run publish.bat

## Usage

Once AppleCaps is running, it will sit in the taskbar tray with the only option `Exit` in the context menu. To switch keyboard layouts, make sure your Windows keyboard layout switch is configured as Ctrl-Shift, then simply press the Caps Lock key as you would on macOS. For activating the Caps Lock function, press and hold the Caps Lock key for more than half a second.

## Configuration

No additional configuration is required after installation. However, ensure that the keyboard layout switching shortcut is set to Ctrl-Shift on Windows to match the utility's expected behavior.

## Contributing

Contributions are what make the open-source community such an amazing place to learn, inspire, and create. Any contributions you make are **greatly appreciated**.

## License

Distributed under the MIT License.

## Acknowledgments

- This project is inspired by the need for a consistent keyboard layout switching experience across macOS and Windows.

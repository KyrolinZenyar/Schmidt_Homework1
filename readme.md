# Daniel Schmidt - Homework 1

This project is a simple two-screen application, built to the requirements given in the project assignment.
The first screen is simply a text field and a button.  The user can type whatever they want into the text field and pressing the button will change the text color of the field to a random color, as well as changing the text in the field to be the RGB values and hex code of the given color.
The second screen opens to a white canvas which the user can draw on with their finger.  There are also three buttons along the bottom, the first of which opens a modal color picker screen.  This screen has sliders for the red, green, and blue values of the stroke color as well as the stroke width, alongside a preview of the color being chosen and save/cancel buttons.  The second button on the drawing canvas screen clears the canvas, while the third will save the image to the device's internal storage (prompting for storage permissions if they have not been previously granted).

# Deployment Instructions

Deployment is a simple matter of copying the provided .apk file to the device and installing it.  It may be necessary to enable the Android setting named something similar to "Install from Unknown Sources".  It is also possible that Google's Play Protect may bring up a warning on installation; if this happens, just choose "Install Anyway".

# Usage Instructions

<img src="https://i.imgur.com/52SQXxY.jpg" alt="Color Changer screen" height="400" width="350"/>

On the Color Changer screen, simply tap in the text field to change the text, or the Change Color button to change the text field's contents and color, as shown below:

<img src="https://i.imgur.com/gZFjFyr.jpg" alt="Changed color" height="400" width="350"/>

Switching to the drawing canvas screen by the simple tabs at the top of the screen allows for finger drawing, as can be seen here:

<img src="https://i.imgur.com/zfbWx2Z.jpg" alt="Drawn canvas" height="400" width="350"/>

Along the bottom of this screen are the buttons for the color picker, clearing, and saving.  Pressing the clear buttons resets the canvas to a white screen, while pressing the save button will save the image to the device's Pictures folder.  Pressing the "Pick Color" button will open up a screen which allows for changing the stroke color and width:

<img src="https://i.imgur.com/ulpX7km.jpg" alt="Color Picker" height="400" width="350"/>

This only changes future drawings on the canvas, not existing strokes, as shown here:

<img src="https://i.imgur.com/7wJFrDj.jpg" alt="Multiple colors" height="400" width="350"/>

# Design Choices

To begin with, I decided to use Xamarin.Forms for my development - this was to get practice using Xamarin, which allows for multi-platform mobile development using one central language with little need for platform-specific code.  I also used three libraries on top of Xamarin for the development of this app: Microsoft's SkiaSharp for on-screen drawing and James Montemagno's CurrentActivity and Permissions plugins in order to get user permissions for saving to the device storage.  These libraries were imported using Nuget, so they are part of the Visual Studio solution; because of this, there are no supporting resources for this app - it is fully self-contained.  The app has its front-end written in Microsoft's XAML, an XML-based front-end language that compiles to the equivalent native code for the mobile device when built.  The back-end code is mostly in the code-behind C# for each screen - all of the code for the color changer screen is written in the equivalent code-behind class.  For the drawing panel, most of the logic is written in the drawing panel code-behind as well; however, the saving is done in a platform-specific class with an interface contained in the cross-platform section, as is required by Xamarin.  The Color Picker modal also has its own code-behind which communicates with the drawing panel code-behind by means of an event listener
which fires when the modal is closed.  This allows the new RGB and stroke values to be communicated back to the drawing panel's code.
I chose to keep most things in the specific page's code-behinds because the code was simple enough and restricted enough to each page that it was simpler and clearer to do it this way - I was also in the process of learning Xamarin so much of my learning what was necessary came from Google searches and documentation, so I didn't want to scatter the code too much such that it muddied the waters even further.

# Notes

I have noticed that sometimes the saved picture will save to the pictures folder but not appear immediately on the gallery.  I am unsure why this is, as it sometimes appears immediately - if the image doesn't appear in the gallery, it can be found in the device's default pictures folder (on my device, Internal Storage > Pictures) and will be named 'Schmidt_HW_1_Image_X" where X is how many images the app has saved to this point.
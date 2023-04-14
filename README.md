# Portable Viewer: C# Image Viewer for PGM, PPM, and PBM Files

## Description

Portable Viewer is a C# application that allows users to view PGM (Portable Graymap), PPM (Portable Pixmap), and PBM (Portable Bitmap) image files. The project provides a straightforward and user-friendly interface for opening and viewing images in these formats. Portable Viewer enables users to convert image data into visual representations, providing a convenient way to visualize the contents of PGM, PPM, and PBM files.

**Key Features:**
- Open and view PGM, PPM, and PBM image files.
- Convert image data to visual representations.
- Simple and user-friendly interface for easy image viewing.
- Designed for developers looking to enhance their image processing skills using C#.
- Provides an example project for reading and displaying PGM, PPM, and PBM files.

Portable Viewer is a handy tool for developers and users who need to view PGM, PPM, and PBM images and inspect their contents. With its straightforward interface and image conversion capabilities, Portable Viewer makes it easy to visualize the contents of these image files.

## Installation

1. Download the Portable Viewer executable (`.exe`) file.
2. Run the executable by double-clicking on it.
3. The executable will check if it is in the correct location. If it is, the application will continue to launch. If it is not, the user will be prompted to confirm if they want to proceed with the installation.
4. If the user selects "Yes" to proceed with the installation, the application will copy itself to the correct location.
5. The application will then add registry entries to associate `.pgm`, `.pbm`, and `.ppm` file extensions with itself as the default viewer.
6. Once the installation is complete, the user can open PGM, PBM, and PPM files by simply double-clicking on them, and they will be opened with Portable Viewer.

Note: Admin privileges may be required for making registry changes during the installation process.

## PBM, PGM, and PPM File Formats
- Portable Graymap Format (PGM);
- Portable Pixmap Format (PPM).
- Portable Bitmap Format (PBM);



### PGM (Portable Graymap)

PGM (Portable Graymap) is a grayscale image file format that represents images in shades of gray, ranging from black to white. PGM files can have different levels of gray, typically represented by integer values ranging from 0 to 255. PGM files are commonly used for images that do not require color, such as diagrams and medical images.

#### P2 Format
P2 format represents the image data in ASCII format, where each pixel value is represented by a decimal number indicating the intensity of gray. The pixel values are arranged in rows and columns. P2 format is human-readable, but it results in larger file sizes compared to the binary P5 format.

Example of P2 format:

```
P2
# This is a comment
3 2
255
0 255 255
255 0 255
```

#### P5 Format
P5 format represents the image data in binary format, where each pixel value is represented by an 8-bit binary value indicating the intensity of gray. The pixel values are stored as binary data without any ASCII representation, resulting in smaller file sizes compared to the ASCII P2 format.

Example of P5 format:

```
P5
# This is a comment
3 2
255
.....bytes here.....
```



### PPM (Portable Pixmap)

PPM (Portable Pixmap) is a color image file format that represents images in RGB format, where each pixel has separate red, green, and blue color channels. PPM files can have different color depths, typically represented by integer values ranging from 0 to 255. PPM files are commonly used for images that require full color representation, such as photographs and digital art.

#### P3 Format
P3 format represents the image data in ASCII format, where each pixel value is represented by a decimal number indicating the intensity of red, green, and blue color channels. The pixel values are arranged in rows and columns. P3 format is human-readable, but it results in larger file sizes compared to the binary P6 format.

Example of P3 format:

```
P3
# This is a comment
3 2
255
0 0 0 255 255 255 255 255 255
255 255 255 0 0 0 255 255 255
```

#### P6 Format
P6 format represents the image data in binary format, where each pixel value is represented by an 8-bit binary value indicating the intensity of red, green, and blue color channels. The pixel values are stored as binary data without any ASCII representation, resulting in smaller file sizes compared to the ASCII P3 format.

Example of P6 format:

```
P6
# This is a comment
3 2
255
.....bytes here.....
```

 


### PBM (Portable Bitmap)

PBM (Portable Bitmap) format is a portable image file format used for storing black and white or 1-bit depth images. PBM files store images in a binary format where each pixel is represented by a single bit, indicating either black or white. PBM format is simple and widely supported, making it suitable for basic image processing tasks.

#### P1 Format

PBM format has two sub-formats: P1 and P4. P1 format represents the image data in ASCII format, where each pixel is represented by a "0" for black and "1" for white, arranged in rows and columns. The P1 format is human-readable, but it results in larger file sizes compared to the binary P4 format.

Example of P1 format:

```
P1
# This is a comment
3 2
0 1 1
1 0 1
```

#### P4 Format

P4 format, also known as raw PBM format, represents the image data in binary format, where each pixel is represented by a single bit, either 0 for black or 1 for white. The P4 format is more compact compared to the ASCII P1 format, resulting in smaller file sizes.

PBM format, including P1 and P4 sub-formats, is commonly used for storing simple black and white images, such as line drawings, diagrams, and text-based images.

Example of P4 format:

```
P4
# This is a comment
3 2
.....bits here.....
```


Portable Viewer supports opening and viewing images in PBM, PGM, and PPM file formats. The application provides a simple and user-friendly interface for visualizing the contents of these image files, making it convenient for users to work with these common image formats.

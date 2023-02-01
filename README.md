# ascii-art-generator
![image](https://user-images.githubusercontent.com/33037271/215918227-6b940782-9528-4426-8d7c-330953ab2f41.png)

C# ASCII art generator generate ASCII art  for images, to html and command prompt figure.

## Features
1. Add Shades from 8 to 16 to display more accurate shades
2. Resize image with it's exact column and rows
3. New Image reader to read image with different format, eg: bmp, jfif, bmp, png etc

### Colored CMD
The Colored CMD add color to the printed ascii art.
Here is an [Micky] example, you can try other images

```powershell
.\bin\Debug\net5.0\ascii-art.exe --colored-cmd micky.bmp 100 100
```
![image](https://user-images.githubusercontent.com/33037271/215918647-d31d644c-82fa-4f85-9ede-c343c278ff9f.png)


### Colored HTML
print the ascii image to the HTML file
```powershell
.\bin\Debug\net5.0\ascii-art.exe --colored-html micky.bmp 100 100
```
![image](https://user-images.githubusercontent.com/33037271/215918773-16426352-7a01-4ffc-8541-b5c6fef3493b.png)

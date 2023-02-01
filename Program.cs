using System;
using System.Drawing;
using System.IO;
using System.Text;


namespace hw1
{
    class Program
    {
        
        static void Main(string[] args)
        {
            
            Console.WriteLine("ASCII Art Generator");
                 
            if(args[0] == "s" || args[0] == "p"){
                bool inverse = false;
                if(args[0] == "s"){
                    inverse = true;
                }
                BaseRequirement bases = new BaseRequirement(input:args[1], inverse:inverse, width:Int16.Parse(args[2]), height:Int16.Parse(args[3]));
                bases.writeConsole();
                if(args.Length == 5){
                    bases.writeFile(args[4]);
                }

            }
            else if(args[0] == "--colored-cmd"){
                ColoredConsole colored_cmd = new ColoredConsole(input:args[1], width:Int16.Parse(args[2]), height:Int16.Parse(args[3]));
            } else if(args[0] == "--text-file"){
                Enhancement textfile1 = new Enhancement(args[1], width:Int16.Parse(args[2]), height:Int16.Parse(args[3]));
                textfile1.writeFile(args[4]);
            } else if(args[0] == "--colored-html") {
                HtmlFile htmlFile1 = new HtmlFile(args[1], width:Int16.Parse(args[2]), height:Int16.Parse(args[3]));
                htmlFile1.writeFile(args[4]);
            } else if(args[0] == "--colored-image"){
                ImageFile imageFile1 = new ImageFile(args[1], width:Int16.Parse(args[2]), height:Int16.Parse(args[3]), args[4]);
            }

        }
    }

    public class BaseRequirement{

        protected string[] shades = {"@","#","%","*","|","-","."," "};
        
        protected Bitmap myBmp;

        protected StringBuilder sb = new StringBuilder();

        public BaseRequirement(string input, bool inverse, int width, int height){
            
            readImage(input);
            resizeImage(height, width);
            Console.WriteLine("Done resize");

            // Step 3: Algorithm
            bmpToAscii(inverse);
            myBmp.Dispose();
        }

        public virtual void writeFile(string output){
            using( StreamWriter file = new StreamWriter(output)){
                file.WriteLine(sb.ToString());
            }
        }

        public void writeConsole(){
            Console.Write(sb.ToString());
        }

        protected BaseRequirement(){}

        //Step 1: Read Image
        protected virtual void readImage(string filename){
            myBmp = (Bitmap)Image.FromFile(filename);
        }

        // Step2: resize image
        protected virtual void resizeImage(int newHeight, int newWidth){

            
            int boxWidth = 0;
            int boxHeight = 0;
            if(newHeight == 0 && newWidth == 0){
                newHeight = myBmp.Height;
                newWidth = myBmp.Width;
                return ;
            }
            else if (newHeight == 0){
                boxWidth = myBmp.Width / newWidth + (myBmp.Width % newWidth == 0 ? 0 : 1);
                boxHeight = boxWidth;
            }
            else if (newWidth == 0) {
                boxHeight = myBmp.Height / newHeight + (myBmp.Height % newHeight == 0 ? 0 : 1);
                boxWidth = boxHeight;
            } else {
                boxHeight = myBmp.Height / newHeight + (myBmp.Height % newHeight == 0 ? 0 : 1);
                boxWidth = myBmp.Width / newWidth + (myBmp.Width % newWidth == 0 ? 0 : 1);
            }

            int row = myBmp.Height / boxHeight;
            int column = myBmp.Width / boxWidth;

            Bitmap outputBitmap = new Bitmap(column, row);

            // Set offset, init r g b sum
            for(int i = 0; i < row; i++){
                for(int j = 0; j < column; j++){
                    int x_offset = boxWidth * j;
                    int y_offset = boxHeight * i;
                    int r_sum = 0;
                    int g_sum = 0;
                    int b_sum = 0;

                    // Average Color inside a box filter
                    for(int x = 0; x < boxWidth; x++){
                        for(int y = 0; y < boxHeight; y++){
                            Color color2 = myBmp.GetPixel(x + x_offset, y + y_offset);
                            b_sum += color2.B;
                            g_sum += color2.G;
                            r_sum += color2.R;
                        }
                    }
                    r_sum = r_sum / (boxHeight*boxWidth);
                    g_sum = g_sum / (boxHeight*boxWidth);
                    b_sum = b_sum / (boxHeight*boxWidth);
                    outputBitmap.SetPixel(j, i, Color.FromArgb(r_sum, g_sum, b_sum));

                }
            }
            myBmp = (Bitmap)outputBitmap.Clone();
            outputBitmap.Dispose();
        }

        public virtual void bmpToAscii(bool inverse){
            // Step 3: Algorithm
            for(int h = 0; h< myBmp.Height; h++){
                for(int w = 0; w < myBmp.Width; w++){
                    Color pixelColor = myBmp.GetPixel(w,h);
                    byte gray = (byte)(.299 * pixelColor.R + .587 * pixelColor.G + .114 * pixelColor.B);
                    Color greyColor = Color.FromArgb(gray, gray, gray);
                    myBmp.SetPixel(w,h, greyColor);

                    double greyNum = greyColor.R/(256/8);
                    int scale_num = 0;
                    scale_num = (int)greyNum;

                    if ((greyNum - (double)scale_num) > (1 + (double)scale_num - greyNum)) {
                         scale_num++; 
                    }
                    if(inverse){
                        scale_num = 7 - scale_num;
                    }
                    sendToOutput(shades[scale_num]); // 7 - scale num for inverse 
                }
                sendToOutput("\n");
            }
        }

        protected void sendToOutput(string data){
            sb.Append(data);
        }

    }
    
    // This includes several features supporting other facny things I made, features are
    // 1. More color Shades, enhanced bmp to ascii
    // 2. Different Image resize algorithm(better one)
    // 3. Enhanced send to output to accept colors as parameter
    // 4. Enhanced bmp to ascii to accept colors as parameter
    // 5. New read image handler to accept many picture format
    public class Enhancement: BaseRequirement{

        public Enhancement(){}
        public Enhancement(string input, int width, int height){
            readImage(input);
            resizeImage(height, width);

            // Step 3: Algorithm
            bmpToAscii();

            myBmp.Dispose();
        }
        protected new string[] shades = {" ", ".", ":", "=", "+", "!", "*", "x", "z", "%", "Z", "X", "$", "&", "#", "@" };

        protected override void resizeImage(int newHeight, int newWidth){
            if(newHeight == 0 && newWidth == 0 ){
                newHeight = myBmp.Height;
                newWidth = myBmp.Width;
            }
            else if(newHeight == 0){
                newHeight = newWidth;
            } else if(newWidth == 0) {
                newWidth = newHeight;
            }

            myBmp = new Bitmap(myBmp, new Size(newWidth, newHeight));
        }

        public void bmpToAscii(){
            
            // Step 3: Algorithm
            for(int h = 0; h< myBmp.Height; h++){
                for(int w = 0; w < myBmp.Width; w++){
                    Color pixelColor = myBmp.GetPixel(w,h);
                    byte gray = (byte)(.299 * pixelColor.R + .587 * pixelColor.G + .114 * pixelColor.B);
                    Color greyColor = Color.FromArgb(gray, gray, gray);
                    myBmp.SetPixel(w,h, greyColor);

                    double greyNum = greyColor.R/(256/16);
                    int scale_num = 0;
                    scale_num = (int)greyNum;
                    if ((greyNum - (double)scale_num) > (1 + (double)scale_num - greyNum)) {
                         scale_num++; 
                    }
                    sendToOutput(shades[scale_num], pixelColor); // 15 - scale num for inverse 
                }
                sendToOutput("\n");
            }
        }

        protected virtual void sendToOutput(string data, Color color = default(Color)){
            sb.Append(data);
        }

        protected override void readImage(string filename){
            Image img = Image.FromFile(filename);
            myBmp = new Bitmap(img);
        }

    }

    // Fill the console with color
    public class ColoredConsole: Enhancement{

         public ColoredConsole(){}
         public ColoredConsole(string input, int width, int height){

            readImage(input);
            resizeImage(height, width);

            // Step 3: Algorithm
            bmpToAscii();

            myBmp.Dispose();
            
        }

        protected override void sendToOutput(string data, Color color = default(Color)){
            Console.ForegroundColor = ClosestConsoleColor(color.R, color.G, color.B);
            Console.Write(data);
        }

        private ConsoleColor ClosestConsoleColor(byte red, byte green, byte blue){
            ConsoleColor target = 0;
            double rr = red, gg = green, bb = blue, delta = double.MaxValue;

            foreach (ConsoleColor colorCode in Enum.GetValues(typeof(ConsoleColor)))
            {
                var n = Enum.GetName(typeof(ConsoleColor), colorCode);
                var color = System.Drawing.Color.FromName(n == "DarkYellow" ? "Orange" : n); // bug fix
                var offset = Math.Pow(color.R - rr, 2.0) + Math.Pow(color.G - gg, 2.0) + Math.Pow(color.B - bb, 2.0);
                if (offset == 0.0)
                    return colorCode;
                if (offset < delta)
                {
                    delta = offset;
                    target = colorCode;
                }
            }
            return target;
        }
    }

    // Generate colored html
    public class HtmlFile: Enhancement{

        public HtmlFile(string input, int width, int height){

            readImage(input);
            resizeImage(height, width);

            // Step 3: Algorithm
            bmpToAscii();

            myBmp.Dispose();

        }

        public override void writeFile(string output){
            using (StreamWriter file = new StreamWriter(output))
            {
                sb.Insert(0, "<html><body style=\"background-color: black; font-size: 8px;\"><pre>");
                sb.Append("</pre></body></html>");
                file.WriteLine(sb.ToString()); // "sb" is the StringBuilder
            }
        }

        protected override void sendToOutput(string data, Color color = default(Color)){
            string hexcolor = ColorTranslator.ToHtml(color);
            if(data != "\n"){
                sb.Append(String.Format("<span style=\"color:{0}\">{1}</span>", hexcolor, data));
            } else if(data == "\n"){
                sb.Append("<br />");
            }
        }


    }
    
    // Generate Image file
    public class ImageFile: Enhancement{ 

        int yCursor = 0;
        int xCursor = 0;
        Size size = new Size(0, 0);

        Bitmap outputBmp;

        Graphics graphics;
        Font font;

        new void writeFile(string output){
            graphics.Flush();
            font.Dispose();
            graphics.Dispose();

            outputBmp.Save(output);
            outputBmp.Dispose();
            
        }

        public ImageFile(string input, int width, int height, string output){

            readImage(input);
            resizeImage(height, width);
            createFileHandler();

            // Step 3: Algorithm
            bmpToAscii(); 

            myBmp.Dispose();
            
            // Modes
           writeFile(output);
            
        }

        protected override void resizeImage(int newHeight, int newWidth){
            if(newHeight == 0 && newWidth == 0 ){
                newHeight = myBmp.Height;
                newWidth = myBmp.Width;
            }
            else if(newHeight == 0){
                newHeight = newWidth;
            } else if(newWidth == 0) {
                newWidth = newHeight;
            }
            size = new Size(newWidth, newHeight);
            myBmp = new Bitmap(myBmp, size);
        }

        void createFileHandler(){
            outputBmp = new Bitmap(width: size.Width * 8, height: size.Height * 8);
            graphics = Graphics.FromImage(outputBmp);
            graphics.FillRectangle(new SolidBrush(Color.FromName("Black")), 0, 0, outputBmp.Width, outputBmp.Height);
            font = new Font("Arial", 8, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
        }

        protected override void sendToOutput(string data, Color color){
            
            graphics.DrawString(data, font, new SolidBrush(color), x:xCursor, y:yCursor);
            if(data == "\n"){
                yCursor += 8;
                xCursor = 0;
            } else {
                xCursor += 8;
            }
            
        }

    }

    /*
    public class VideoFile{
        public VideoFile(){

            // Gets the frame at 5th second of the video.
           int i = 0;
            var file = MediaFile.Open(@"C:\videos\movie.mp4");
            while(file.Video.TryReadNextFrame(out var imageData))
            {
                imageData.ToBitmap().Save($@"C:\images\frame_{i++}.png");
                // See the #Usage details for example .ToBitmap() implementation
                // The .Save() method may be different depending on your graphics library
            }
        }

        public unsafe Bitmap ToBitmap(this ImageData bitmap)
        {
            fixed(byte* p = bitmap.Data)
            {
                return new Bitmap(bitmap.ImageSize.Width, bitmap.ImageSize.Height, bitmap.Stride, PixelFormat.Format24bppRgb, new IntPtr(p));
            }
        }
    }*/
    
}

using Tesseract;
using Spentir.Application.Services.Interfaces;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text.RegularExpressions;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.PixelFormats;

namespace Spentir.Application.Services
{
    public sealed class TesseractOcrService : IOcrService
    {
        public async Task<string> ExtractTextAsync(Stream imageStream, CancellationToken cancellationToken = default)
        {
            var tempFile = Path.ChangeExtension(Path.GetTempFileName(), ".png"); // Ensure valid image extension

            await using (var fs = File.Create(tempFile))
            {
                await imageStream.CopyToAsync(fs, cancellationToken);
            }

            Console.WriteLine($"Temp file size: {new FileInfo(tempFile).Length} bytes");

            try
            {
                // Validate and preprocess the image
                if (!ValidateImage(tempFile))
                {
                    throw new InvalidOperationException("Invalid or unsupported image format.");
                }

                PreprocessImage(tempFile);

                using var engine = new TesseractEngine(@"./tessdata", "eng", EngineMode.Default);

                // Configure Tesseract for better receipt recognition
                engine.SetVariable("tessedit_char_whitelist", "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz.,:-/$ ");
                engine.SetVariable("user_defined_dpi", "300");

                using var img = Pix.LoadFromFile(tempFile);
                using var page = engine.Process(img, PageSegMode.SingleBlock);

                var rawText = page.GetText();

                // Apply post-processing to clean up OCR output
                return rawText;
            }
            finally
            {
                File.Delete(tempFile);
            }
        }

        private bool ValidateImage(string filePath)
        {
            try
            {
                using var image = SixLabors.ImageSharp.Image.Load(filePath);
                // Check resolution and format
                return image.Width > 100 && image.Height > 100;
            }
            catch
            {
                return false;
            }
        }

        private void PreprocessImage(string filePath)
        {
            using var image = SixLabors.ImageSharp.Image.Load<Rgba32>(filePath);

            // Convert to grayscale
            image.Mutate(x => x.Grayscale());

            // Apply deskewing
            image.Mutate(x => x.AutoOrient());

            // Enhance contrast
            image.Mutate(x => x.Contrast(1.5f));

            // Apply sharpening
            image.Mutate(x => x.GaussianSharpen(2.5f));

            // Apply binarization
            image.Mutate(x => x.BinaryThreshold(0.1f));

            // Apply edge detection instead of dilation
            image.Mutate(x => x.DetectEdges());


            // Save the preprocessed image back to the file
            image.Save(filePath); // File extension is now valid
        }
    }
}
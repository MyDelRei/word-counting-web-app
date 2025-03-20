using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WordCounting.Models;
using System.IO;
using System.Web;
using Microsoft.AspNetCore.Routing.Constraints;
using static System.Net.Mime.MediaTypeNames;
using System.Text.Json;


namespace WordCounting.Controllers
{
    public class HomeController : Controller
    {
        
        public IActionResult Index()
        {
            return View(new UploadModel()); 
        }

        [HttpPost]
        public async Task<IActionResult> UploadFile(UploadModel model)
        {
            if (model.UploadFile != null)
            {
                try
                {
                    string fileName = Path.GetFileName(model.UploadFile.FileName);
                    string uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");

                    Directory.CreateDirectory(uploadsPath); 

                    string filePath = Path.Combine(uploadsPath, fileName);
                    System.Diagnostics.Debug.WriteLine($"Saving file to: {filePath}");

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.UploadFile.CopyToAsync(stream);
                    }

                    string fileContent = await System.IO.File.ReadAllTextAsync(filePath);
                    return Json(new { success = true, Content = fileContent });
                }
                catch (Exception ex)
                {
                    string errorDetails = $"File Upload Error: {ex.Message} - Stack Trace: {ex.StackTrace}";
                    System.Diagnostics.Debug.WriteLine(errorDetails);
                    return Json(new { success = false, Message = errorDetails });
                }
            }
            return Json(new { success = false, Message = "No file uploaded." });
        }

        [HttpGet]
        public IActionResult Counting(string word)
        {
            if (string.IsNullOrEmpty(word))
            {
                return Ok();
            }
            Dictionary<string,int> repeatword = countrepeatword(word);
            int number_of_word= countwordnumber(word);

            return Ok(new { number_of_word , repeatword });
            
        }

        public static Dictionary<string, int> countrepeatword(string word)
        {
            string[] words = word.Split(' ');
            Dictionary<string, int> wordCount = new Dictionary<string, int>();

            foreach (string w in words)
            {
                if (wordCount.ContainsKey(w))
                    wordCount[w]++;
                else
                    wordCount[w] = 1;
            }
            return wordCount;
        }

        public static int countwordnumber(string word)
        {
            int wordCount = 0;
            bool inWord = false;

            // Traverse through each character in the string
            foreach (char c in word)
            {
                // Check if the character is a letter (a-z or A-Z)
                if (Char.IsLetter(c))
                {
                    if (!inWord)
                    {
                        wordCount++;  // We encountered the start of a new word
                        inWord = true;
                    }
                }
                else
                {
                    // If it's not a letter (space, number, symbol), we're no longer in a word
                    inWord = false;
                }
            }

            return wordCount;
        }



    }
}

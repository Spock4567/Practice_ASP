using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Practice_ASP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StringProcessingController : ControllerBase
    {
        private readonly StringProcessorService _processorService;

        public StringProcessingController(StringProcessorService processorService)
        {
            _processorService = processorService;
        }

        [HttpGet]
        public async Task<IActionResult> ProcessString([FromQuery] string str)
        {
            try
            {
                // Проверка на неподходящие символы
                var unsuitableChars = _processorService.GetUnsuitableChars(str);
                if (unsuitableChars.Count > 0)
                {
                    return StatusCode(400, $"Строка содержит неподходящие символы: {string.Join(", ", unsuitableChars)}");
                }

                // Основная обработка
                string processedString = _processorService.StringProcessing(str);

                // Подготовка ответа
                var response = new StringProcessingResponse
                {
                    ProcessedString = processedString,
                    CharRepetitions = _processorService.GetCharsRepetitions(processedString),
                    LongestVowelSubstring = _processorService.GetLongestVowelSubstring(processedString),
                    QuickSortedString = _processorService.QuickSortString(processedString),
                    TreeSortedString = _processorService.TreeSortString(processedString)
                };

                // Удаление случайного символа
                string shortenedString = await _processorService.DeleteRandomChar(processedString);
                response.ShortenedString = shortenedString;

                // Находим удаленный символ
                for (int i = 0; i < processedString.Length; i++)
                {
                    if (i >= shortenedString.Length || processedString[i] != shortenedString[i])
                    {
                        response.RemovedChar = processedString[i];
                        response.RemovedCharIndex = i + 1; // индекс символа если начинать отсчёт с 1
                        break;
                    }
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Произошла ошибка: {ex.Message}");
            }
        }
    }
}

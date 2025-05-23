﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Practice_ASP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StringProcessingController : ControllerBase
    {
        private readonly StringProcessorService _processorService;
        private readonly RequestLimiterService _requestLimiter;

        public StringProcessingController(StringProcessorService processorService,
            RequestLimiterService requestLimiter)
        {
            _processorService = processorService;
            _requestLimiter = requestLimiter;
        }

        [HttpGet]
        public async Task<IActionResult> ProcessString([FromQuery] string str)
        {
            // Проверка лимита запросов
            if (!_requestLimiter.TryAcquireSlot())
            {
                return StatusCode(StatusCodes.Status503ServiceUnavailable,
                    "Превышен лимит запросов. Сервис недоступен");
            }

            try
            {
                // Проверка на неподходящие символы
                var unsuitableChars = _processorService.GetUnsuitableChars(str);
                if (unsuitableChars.Count > 0)
                {
                    return StatusCode(400, $"Строка содержит неподходящие символы: {string.Join(", ", unsuitableChars)}");
                }

                //Проверка на наличие строки в чёрном списке
                bool stringIsInBlackList = _processorService.StringIsInBlackList(str);
                if (stringIsInBlackList)
                {
                    return StatusCode(400, "Строка находится в чёрном списке!");
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
            finally
            {
                //освобождаем слот
                _requestLimiter.ReleaseSlot();
            }
        }
    }
}

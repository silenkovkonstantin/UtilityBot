using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types.Enums;
using Microsoft.VisualBasic;
using System.Threading;
using UtilityBot.Services;

namespace UtilityBot.Controllers
{
    public class TextMessageController
    {
        private readonly IStorage _memoryStorage;
        private readonly ITelegramBotClient _telegramClient;
        private readonly IAddition _calculator;

        public TextMessageController(ITelegramBotClient telegramBotClient, IStorage memoryStorage, IAddition calculator)
        {
            _telegramClient = telegramBotClient;
            _memoryStorage = memoryStorage;
            _calculator = calculator;
        }

        public async Task Handle(Message message, CancellationToken ct)
        {
            switch (message.Text)
            {
                case "/start":

                    // Объект, представляющий кноки
                    var buttons = new List<InlineKeyboardButton[]>();
                    buttons.Add(new[]
                    {
                        InlineKeyboardButton.WithCallbackData($" Количество символов" , $"amount"),
                        InlineKeyboardButton.WithCallbackData($" Сумма чисел" , $"sum")
                    });

                    // передаем кнопки вместе с сообщением (параметр ReplyMarkup)
                    await _telegramClient.SendTextMessageAsync(message.Chat.Id, $"<b>  Наш бот подсчитывает количество символов в тексте или сумму чисел.</b> {Environment.NewLine}",
                        cancellationToken: ct, parseMode: ParseMode.Html, replyMarkup: new InlineKeyboardMarkup(buttons));

                    break;
                default:
                    string userFunctionCode = _memoryStorage.GetSession(message.Chat.Id).FunctionCode; // Здесь получим выполняемую функцию из сессии пользователя

                    switch (userFunctionCode)
                    {
                        case "amount":
                            await _telegramClient.SendTextMessageAsync(message.Chat.Id, $"Длина сообщения: {message.Text.Length} знаков", cancellationToken: ct);
                            //await _telegramClient.SendTextMessageAsync(message.Chat.Id, "Отправьте текст для подсчета количества символов.", cancellationToken: ct);
                            break;
                        case "sum":
                            try
                            {
                                double sum = _calculator.AddNumbers(message.Text); // Используем сервис сложения чисел
                                await _telegramClient.SendTextMessageAsync(message.Chat.Id, $"Сумма чисел: {sum}", cancellationToken: ct);
                                //await _telegramClient.SendTextMessageAsync(message.Chat.Id, "Отправьте несколько чисел через пробел для подсчета их суммы.", cancellationToken: ct);
                            }
                            catch (ArgumentException ex)
                            {
                                await _telegramClient.SendTextMessageAsync(message.Chat.Id, "Неверный ввод. Отправьте несколько чисел через пробел для подсчета их суммы.", cancellationToken: ct);
                            }
                            break;


                    }
                        
                    break;
            }
        }
    }
}
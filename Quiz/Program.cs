using Quiz.Brokers.SubjectBrokers;
using Quiz.Brokers.UserBroker;
using Quiz.Brokers.UserBrokers;
using Quiz.Models;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace Quiz
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string token = "6746544742:AAFUTagbEZ4q3ZgWUIEcUDrxKTPgO-S34WI";
            TelegramBotClient botClient = new TelegramBotClient(token);

            botClient.StartReceiving(
                updateHandler: UpdateHandlerAsync,
                pollingErrorHandler: ErrorHandlerAsync);
            Console.ReadKey();
        }

        private static Task ErrorHandlerAsync(ITelegramBotClient client, Exception exception, CancellationToken token)
        {
            var ErrorMessage = exception switch
            {
                ApiRequestException apiRequestException
                    => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            Console.WriteLine(ErrorMessage);
            return Task.CompletedTask;
        }

        static ISubjectBroker subjectBroker = new SubjectBroker();
        private static async Task UpdateHandlerAsync(ITelegramBotClient client, Update update, CancellationToken token)
        {
            //return;

            if (update.CallbackQuery is not null)
            {
                string message = update.CallbackQuery.Data;

                switch (message.Substring(3, 3))
                {
                    case "Sub":
                        await OnCallBackQuerySubjectAsync(client, update.CallbackQuery);
                        break;
                }


            }
            else if (update.Message.Type is MessageType.Text)
            {
                string text = update.Message.Text;
                if (text == "/start")
                {
                    await OnSendStartAsync(client, token, update.Message);
                }
                else if (text.Substring(0, 3) == "sub")
                {
                    string subName = text.Substring(4);

                    await subjectBroker.InsertSubjectAsync(subName);
                }
            }



        }

        private static async Task OnCallBackQuerySubjectAsync(ITelegramBotClient client, CallbackQuery callbackQuery)
        {
            switch (callbackQuery.Data.Substring(0, 3))
            {
                case "ins":
                    await client.SendTextMessageAsync(chatId: callbackQuery.Message.Chat.Id, "Subject name : {sample sub%SubjectName}");
                    break;
            }
        }

        static IUserBroker userBroker = new UserBroker();
        private static async Task OnSendStartAsync(ITelegramBotClient client, CancellationToken token, Message message)
        {
            User tgUser = message.From;

            Role? storedUserRole = await userBroker.LoginAsync(tgUser.Id);
            if (storedUserRole is null)
            {
                bool isRegistered = await userBroker.RegisterUserAsync(
                    new TgUser()
                    {
                        TgId = tgUser.Id,
                        FullName = tgUser.FirstName + " " + tgUser.LastName,
                        UserName = tgUser.Username,
                        UserRole = Role.USER,
                    }
                    );
            }

            else if (storedUserRole is Role.ADMIN)
            {
                await client.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                    text: "Quyidagilardan birini tanlang",
                    replyMarkup: new InlineKeyboardMarkup(
                        new InlineKeyboardButton[][]
                        {
                            new InlineKeyboardButton[]
                            {
                                InlineKeyboardButton.WithCallbackData("Add subject", "insSub"),
                                InlineKeyboardButton.WithCallbackData("Get subjects", "getSub")
                            }
                        }
                        ));
            }

            else
            {

            }

        }
    }
}
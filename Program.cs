using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types;
using Telegram.Bot.Exceptions;
using TgBot.Models;
using TgBot.ApiRequests;


// BOR FOYDALANUVCHINI HEMIS ID VA PAROLI 

// Hemis
// 31231102346
// AC0845087

namespace TgBotVetMedIst_uchun
{
    internal class Program
    {
        static void Main(string[] args)
        {

            using var context = new ApplicationDbContext();

            // Create the database if it doesn't exist
            context.Database.EnsureCreated();

            List<BotClientData> botClientList = new();

            // t.me/allaqanday_bot
            var client = new TelegramBotClient("6661004577:AAGWwA9BtHmulGhOMfIo91-TBmFmzL0pj5s");

            Console.WriteLine(index());


            string index()
            {

                var botModelss = context.BotUserDB.ToList();

                foreach (BotUserData item in botModelss)
                {
                    BotClientData botClientData = new BotClientData()
                    {
                        _telNymber = item.UserTelNum,
                        botStatus = 6,
                        _login = "ok",
                        _parol = "ok",
                        token = item.Api_Access_Token,
                        chatId = item.UserId,
                        //pageNumber = 0,
                        //bookName = "",
                        //_inlineBotton_ok = false,
                        avtorizatsiya = "ok"
                    };
                    botClientList.Add(botClientData);
                }


                using CancellationTokenSource cts = new();

                ReceiverOptions receiverOptions = new()
                {
                    AllowedUpdates = Array.Empty<UpdateType>() // receive all update types except ChatMember related updates
                };

                client.StartReceiving(
                    updateHandler: HandleUpdateAsync,
                    pollingErrorHandler: HandlePollingErrorAsync,
                    receiverOptions: receiverOptions,
                    cancellationToken: cts.Token
                );

                var me = client.GetMeAsync(); // cts.Token

                Console.WriteLine($"Start listening for @{me}");
                Console.ReadLine();

                // Send cancellation request to stop bot
                cts.Cancel();
                Console.WriteLine("ishlamoqda...");

                return "index_result__... ";
            }


            async Task HandleUpdateAsync(ITelegramBotClient client, Update update, CancellationToken cancellationToken)
            {

                var messagee = update.Message != null ? update.Message : update.CallbackQuery?.Message != null ? update.CallbackQuery.Message : null;
                if (messagee == null)
                {
                    return;
                }

                //await client.SendTextMessageAsync(
                //            chatId: messagee.Chat.Id,
                //            text: "nima bolsilsa ham chiqar!"
                //        );


                BotClientData botClientData = new BotClientData()
                {
                    _telNymber = string.Empty,
                    botStatus = 1,                      // = 1;
                    _login = string.Empty,
                    _parol = "",
                    token = "",
                    chatId = messagee.Chat.Id,
                    //pageNumber = 0,
                    //bookName = "",
                    //_inlineBotton_ok = false,
                    avtorizatsiya = string.Empty
                };

                //Console.WriteLine(botClientData);

                foreach (var value in botClientList)
                {
                    if (value.chatId == messagee.Chat.Id)
                    {
                        botClientData = value;
                        goto key;
                    }
                }
                botClientList.Add(botClientData);

            key:
                // Only process Message updates: https://core.telegram.org/bots/api#message
                if (update.Message is not { } message)
                {
                    if (update.CallbackQuery is { } callback)
                    {
                        //Console.WriteLine($"message type: {callback.Data}");

                        if (callback.Data[0] == '_')
                        {
                            try
                            {

                                // $"_o{num(pageNumber)}{num(booksRequest.result.totalPages)}{searchBookName}
                                string data = callback.Data;

                                //Console.WriteLine(data);
                                int pageNumber = int.Parse(data.Substring(2, 6));
                                int pageTotals = int.Parse(data.Substring(8, 6));
                                string searchBook = data.Substring(14);


                                if (data[1] == 'o' && pageNumber > 0)
                                {
                                    /*await*/
                                    BotStatus6_1(true, callback.Message.MessageId, searchBook, pageNumber - 1);               // await o'chirildi
                                }

                                else if (data[1] == 'k' && pageNumber + 1 < pageTotals)
                                {
                                    /*await*/
                                    BotStatus6_1(true, callback.Message.MessageId, searchBook, pageNumber + 1);               // await o'chirildi
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex);
                            }

                        }
                        else
                        {
                            for (int i = 0; i < 10; i++)
                            {
                                if (callback.Data.StartsWith($"callback{i}"))
                                {
                                    string fileId = callback.Data.Substring(10);
                                    //Console.WriteLine(fileId);

                                    RootBook bookData = await ApiRequest_BookFile.RequestBook(fileId, botClientData.token);

                                    //$"{callCoverExt}&{callBookId}&{callBookName}&{callBookUrl}&{callBookSize}&{callBookExt}&{callCoverUrl}"

                                    if (bookData == null)
                                    {
                                        await client.SendTextMessageAsync(
                                            chatId: botClientData.chatId,
                                            text: "uzur yuklay omladim!"
                                            );
                                        return;
                                    }
                                    String[] dataa = new String[7];
                                    dataa[0] = bookData.result.coverImage.extension.ToString();
                                    dataa[1] = fileId;
                                    dataa[2] = bookData.result.book_Title.ToString();
                                    dataa[3] = bookData.result.book_File.fileUrl.ToString();
                                    dataa[4] = bookData.result.book_File.size.ToString();
                                    dataa[5] = bookData.result.book_File.extension.ToString();
                                    dataa[6] = bookData.result.coverImage.fileUrl.ToString();

                                    BotStatus7(i, dataa);
                                }
                            }

                        }
                    }

                    return;
                }


                //Console.WriteLine($"message type: {update.CallbackQuery?.Data}  {message.Type} : {MessageType.Contact} : {message.Contact?.PhoneNumber}");
                Console.WriteLine($"Received a '{message.Text}{message.Contact?.PhoneNumber}' message in chat {message.Chat.Id}.");

                // kontacni so'rash                                             1 - 1  to 2
                async Task BotStatus1()
                {
                    botClientData.botStatus = 2;
                    ReplyKeyboardMarkup markup =
                        new ReplyKeyboardMarkup
                            (KeyboardButton.WithRequestContact("Kontaktni yuborish!"));
                    markup.ResizeKeyboard = true;
                    await client.SendTextMessageAsync(
                            chatId: message.Chat.Id,
                            text: "Assalomu alaykum!\nBotni ishlatish uchun Kontaktingizni yuborib ro'yxatdan o'ting!\n\"Kontaktni yuborish!\" tugmasi ekranni pastgi qismida!",
                            replyMarkup: markup
                    );
                }

                // kontactni tekshirib hemis yoki emailni so'rash                 2 - 2  to 201
                async Task BotStatus2()
                {
                    if (message.Contact?.PhoneNumber == null)
                    {
                        ReplyKeyboardMarkup markup =
                        new ReplyKeyboardMarkup
                            (KeyboardButton.WithRequestContact("Kontaktni yuborish!"));
                        markup.ResizeKeyboard = false;
                        await client.SendTextMessageAsync(
                            chatId: message.Chat.Id,
                            text: "Bot hozir kontaktingizni yuborishningizni kutmoqda!\n\"Kontaktni yuborish!\" tugmasini ustiga bosing va yuborishga ruxsat bering!",
                            //text: "Bot hozir kontaktingizni yuborishningizni kutmoqda!\nOsonroq topishingiz uchun\"Kontaktni yuborish!\" kattaroq ko'rinishga o'zgardi\n\"Kontaktni yuborish!\" ustiga bosing va yuborishga ruxsat bering!",
                            replyMarkup: markup
                        );
                    }
                    else if (message.Contact.UserId != message.Chat.Id)
                    {
                        ReplyKeyboardMarkup markup =
                        new ReplyKeyboardMarkup
                            (KeyboardButton.WithRequestContact("Kontaktni yuborish!"));
                        markup.ResizeKeyboard = false;
                        await client.SendTextMessageAsync(
                            chatId: message.Chat.Id,
                            text: "Bu kontakt sizniki emasligi aniqlandi!\r\nIltimos o'z kontaktingizni yuboring👇",
                            //text: "Bot siz manashu yozayotgan Telegram Account kontaktini olgandan keyin keyinigi qadamga o'tadi!\n\"Kontaktni yuborish!\" ustiga bosing va yuborishga ruxsat bering!",
                            replyMarkup: markup
                        );
                    }
                    else
                    {
                        // contact malumotlari kelganda
                        botClientData._telNymber = message.Contact.PhoneNumber;

                        botClientData.botStatus = 201;

                        ReplyKeyboardMarkup replyKeyboardMarkup = new(new[]
                        {
                                new KeyboardButton[] { "Hemis", "Email" },
                        })
                        {
                            ResizeKeyboard = true
                        };

                        await client.SendTextMessageAsync(
                            chatId: message.Chat.Id,
                            text: "Kontakt qabul qilindi!",
                            replyMarkup: replyKeyboardMarkup,
                            cancellationToken: cancellationToken);

                        await client.SendTextMessageAsync(
                           chatId: message.Chat.Id,
                           text: "SamDU talabasi ekanligingizni tekshirish uchun e-library.samdu.uz tizimi Email yoki hemis tekshiruv tizimidan o'tish uchun\nEmail yoki Hemis\n tanlang!",
                           replyMarkup: replyKeyboardMarkup,
                           cancellationToken: cancellationToken);

                        //Console.WriteLine(sentMessage.Text);

                        //await client.SendTextMessageAsync(
                        //    chatId: message.Chat.Id,
                        //    text: "loginini kiriting!"
                        //);

                    }
                }

                // hemis yoki email kiritlganini tekshirish va loginni kritish      2_1 - 201   to  3
                async Task BotStatus2_1()
                {
                    string text = message.Text;

                    if (text == "Hemis" || text == "Email")
                    {
                        botClientData.botStatus = 3;

                        botClientData.avtorizatsiya = text;

                        await client.SendTextMessageAsync(
                            chatId: message.Chat.Id,
                            text: $"{text} tanlandi!",
                            replyMarkup: new ReplyKeyboardRemove()
                        );

                        await client.SendTextMessageAsync(
                            chatId: message.Chat.Id,
                            text: $"{text} loginini kiriting!"
                        );
                    }
                    else
                    {
                        ReplyKeyboardMarkup replyKeyboardMarkup = new(new[]
                        {
                                new KeyboardButton[] { "Hemis", "Email" },          //  ☎️
                        });
                        //{
                        //    ResizeKeyboard = true
                        //};
                        await client.SendTextMessageAsync(
                            chatId: message.Chat.Id,
                            text: "Xato ma'lumot kiritildi!\nHemis yoki Email tanlang!",
                             replyMarkup: replyKeyboardMarkup
                        );
                    }
                }

                // loginni olib parolni so'rash                                     3 - 3  to  4
                async Task BotStatus3()
                {
                    botClientData.botStatus = 4;

                    botClientData._login = message.Text;

                    await client
                       .SendTextMessageAsync(message.Chat.Id, $"{botClientData.avtorizatsiya} parolni kiriting!");
                }

                //parolni olib avtorizatsiya tekshiruvidan tekshirish  va qidirayotgan kitobni so'rash    4 - 4  to 6 or 201
                async Task BotStatus4()
                {
                    botClientData._parol = message.Text;

                    func(botClientData._login, botClientData._parol);

                    async Task func(string? login, string? parol)
                    {
                        BotUserData botModel = await ApiRequest_User_Aut.Request(login, parol, botClientData.avtorizatsiya == "Hemis");
                        if (botModel == null)
                        {
                            botClientData.botStatus = 201;

                            ReplyKeyboardMarkup replyKeyboardMarkup = new(new[]
                                {
                                    new KeyboardButton[] { "Hemis", "Email" }, //☎️
                                }
                            )
                            {
                                ResizeKeyboard = true
                            };


                            await client.SendTextMessageAsync(
                                chatId: message.Chat.Id,
                                text: "Tekshiruv muvoffaqiyatsiz yakunlandi.\nQayta urinib ko'ring.\nEmail yoki Hemisni tanlang!",
                                //text: "Tekshiruvdan o'tish muvaffaqiyatsiz yakunlandi! Qaytadan xarakat qilib ko'ring!\nEkranni pastgi qismidan tanlang!\nEmail yoki Hemis",
                                replyMarkup: replyKeyboardMarkup,
                                cancellationToken: cancellationToken);

                            return;
                        }

                        botClientData.token = botModel.Api_Access_Token;
                        botModel.UserId = message.Chat.Id;
                        botModel.UserName = message.Chat.FirstName;
                        botModel.UserLast = message.Chat.LastName;
                        botModel.UserTelNum = botClientData._telNymber;
                        botModel.UserNameSearch = message.Chat.Username;

                        try
                        {
                            context.BotUserDB.Add(botModel);
                            context.SaveChanges();
                        }
                        catch (Exception ex)
                        {
                            //Console.WriteLine(ex);

                            //await client.SendTextMessageAsync(
                            //chatId: message.Chat.Id,
                            //text: $"Malumotlarni saqlashda xoto yuz berdi!\nAmmo botdan foydalanishingiz mumkin!\nError: {ex.Message}"
                            //);
                        }

                        // bazaga saqlanganda
                        //botClientData.botStatus = 5;

                        botClientData.botStatus = 6;

                        await client.SendTextMessageAsync(
                            chatId: message.Chat.Id,
                            text: "Tabriklaymiz!\nEndi siz ham Bot foydalanuvchisisiz!"
                        //text: "Tizimdan muvofaqiyyatli o'tdingiz!"
                        );
                        await client.SendTextMessageAsync(
                            chatId: message.Chat.Id,
                            text: "Qidirayotgan kitob nomini kriting!",
                            cancellationToken: cancellationToken
                        );
                    }

                    //if (await func(botClientData._login, botClientData._parol))
                    //{

                    //}
                    //else
                    //{

                    //}
                }

                // qiditayotgan kitobni kiritish so'rovi                        //5 - 5    botStatus = 5 bo'maydi umuman!
                async Task BotStatus5()
                {
                    botClientData.botStatus = 6;

                    await client.SendTextMessageAsync(
                         chatId: botClientData.chatId,
                         text: "Qidirilayotgan kitob nomini kiriting:",
                         cancellationToken: cancellationToken
                     );
                }

                // yangi ro'yxat hosil qilish  yoki mavjudini o'zgartirish kerakligini aniqlash va  ro'yxatni hosil qilish uchun so'rov yuborish    6- 6 to 6      call BotStatus6_1
                async Task BotStatus6(bool editing, string searchBookName, int pageNumber)
                {
                    //botClientData.botStatus = 8;
                    //botClientData.bookName = message.Text;
                    // botClientData.chatId = message.Chat.Id;
                    /*await */
                    if (searchBookName == null)
                    {
                        await client.SendTextMessageAsync(
                            chatId: botClientData.chatId,
                            text: "Qidirilayotgan kitob nomini kiriting:",
                            replyMarkup: new ReplyKeyboardRemove(),
                            cancellationToken: cancellationToken
                        );
                    }
                    else
                    {
                        BotStatus6_1(false, 0, searchBookName, pageNumber);                           // await o'chirildi
                    }
                }

                // yangi ro'yxat hosil qilish yoki mavjudini hosil qilish funksiyasi                                    
                async Task BotStatus6_1(bool editing, int messageId, string searchBookName, int pageNumber)
                {
                    try
                    {
                        //botClientData._inlineBotton_ok = true;
                        // search book request
                        Books booksRequest = await ApiRequest_BookSearch.RequestBooks(searchBookName, pageNumber, 10);

                        string kitoblarNomi;

                        if (booksRequest.result.totalCount == 0)
                        {
                            kitoblarNomi = "Bunday kitob kutubxona bazasidan topilmadi!😔";
                        }
                        else
                        {
                            //kitoblarNomi = $"{booksRequest.result.totalCount} ta natija topildi!\nva ulardan:\n";
                            kitoblarNomi = $"Topilgan natijalar - {booksRequest.result.totalCount} ta\n";
                        }


                        //Console.WriteLine(booksRequest);

                        int bookCount = booksRequest.result.items.Count;

                        InlineKeyboardButton[][] button = new InlineKeyboardButton[3][];

                        button[0] = new InlineKeyboardButton[(bookCount + 1) / 2];
                        button[1] = new InlineKeyboardButton[bookCount - button[0].Length];
                        string keyingi;
                        if ((2 + pageNumber) * 10 < booksRequest.result.totalCount)
                        {
                            keyingi = $"{(1 + pageNumber) * 10 + 1} - {(2 + pageNumber) * 10}";
                        }
                        else
                        {
                            if (booksRequest.result.totalCount % 10 == 1)
                            {
                                keyingi = $"{booksRequest.result.totalCount}";
                            }
                            else
                            {
                                keyingi = $"{(1 + pageNumber) * 10 + 1} - {booksRequest.result.totalCount}";
                            }

                        }



                        if (pageNumber == 0)
                        {
                            if (pageNumber + 1 >= booksRequest.result.totalPages)
                            {
                                button[2] = new InlineKeyboardButton[0];
                            }
                            else
                            {
                                button[2] = new InlineKeyboardButton[1];

                                button[2][0] = InlineKeyboardButton.WithCallbackData(text: keyingi, callbackData: $"_k{Num(pageNumber)}{Num(booksRequest?.result?.totalPages)}{StrTrimAndLengCorect(searchBookName)}");
                            }
                        }
                        else
                        {
                            if (pageNumber + 1 >= booksRequest.result.totalPages)
                            {
                                button[2] = new InlineKeyboardButton[1];
                                button[2][0] = InlineKeyboardButton.WithCallbackData(text: $"{(pageNumber - 1) * 10 + 1} - {pageNumber * 10}", callbackData: $"_o{Num(pageNumber)}{Num(booksRequest?.result?.totalPages)}{StrTrimAndLengCorect(searchBookName)}");
                            }
                            else
                            {
                                button[2] = new InlineKeyboardButton[2];
                                button[2][0] = InlineKeyboardButton.WithCallbackData(text: $"{(pageNumber - 1) * 10 + 1} - {pageNumber * 10}", callbackData: $"_o{Num(pageNumber)}{Num(booksRequest?.result?.totalPages)}{StrTrimAndLengCorect(searchBookName)}");
                                button[2][1] = InlineKeyboardButton.WithCallbackData(text: keyingi, callbackData: $"_k{Num(pageNumber)}{Num(booksRequest?.result?.totalPages)}{StrTrimAndLengCorect(searchBookName)}");
                            }
                        }

                        for (int i = 0; i < bookCount; i++)
                        {
                            var fileSizeOrNull = booksRequest?.result?.items?[i]?.book_File?.size;
                            string fileSize = FileSize(fileSizeOrNull.GetType() == typeof(long) ? fileSizeOrNull : 0);

                            var callBookId = booksRequest.result.items[i].id;

                            kitoblarNomi += $"\n{i + 1 + 10 * pageNumber}. {booksRequest?.result?.items?[i]?.book_Title} {fileSize}";
                            string callBackDataa = $"callback{i}&{callBookId}";// &{callBookName}&{callBookUrl}&{callBookSize}&{callBookExt}&{callCoverUrl}&{callCoverExt}";

                            if (i < button[0].Length)
                            {
                                button[0][i] = InlineKeyboardButton.WithCallbackData(text: $"{i + 1 + 10 * pageNumber}", callbackData: callBackDataa);
                            }
                            else
                            {
                                button[1][i - button[0].Length] = InlineKeyboardButton.WithCallbackData(text: $"{i + 1 + 10 * pageNumber}", callbackData: callBackDataa);
                            }
                        }

                        var markup = new InlineKeyboardMarkup(button);


                        if (editing)
                        {
                            await client.EditMessageTextAsync(
                                chatId: botClientData.chatId,
                                messageId: messageId,
                                text: kitoblarNomi,
                                replyMarkup: markup,
                                cancellationToken: cancellationToken
                            );
                        }
                        else
                        {
                            await client.SendTextMessageAsync(
                                chatId: botClientData.chatId,
                                text: kitoblarNomi,
                                replyMarkup: markup,
                                cancellationToken: cancellationToken
                            );
                        }

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                }

                // boshqa kitob qidirish uchun mavjud ro'yxatni o'chirish                                              
                async Task BotStatus6_2(int _messageId)
                {
                    //botClientData.pageNumber = 0;
                    //botClientData._inlineBotton_ok = false;
                    await client.DeleteMessageAsync(
                            chatId: botClientData.chatId,
                            messageId: _messageId
                        );
                    //await BotStatus5();
                }

                // tanlangan kitobni yuklash
                async Task BotStatus7(int index, string[] data)
                {
                    try
                    {
                        // coverEXt fileId bookname, bookurl, booksize, bookext, coverurl
                        //      0       1       2       3          4        5       6

                        //DownloadedFileAdressToTg download_fileInTg = await context.FileTgAdressDb.FirstOrDefault(b => b.fileId.Equals( botClientData.booksRequest.result.items[index].id));
                        var download_fileInTg = context.FileTgAdressDb
                            .Where(item => item.fileId.ToString() == data[1])
                            .FirstOrDefault();

                        Message tgMessage = null;

                        Message yulanmoqda = await client.SendTextMessageAsync(
                                        chatId: botClientData.chatId,
                                        text: "yuklanmoqda...",
                                        cancellationToken: cancellationToken
                                    );

                        if (int.Parse(data[4]) >= 52428800)       // 50MB+
                        {
                            //       0           1               2           3               4           5               6
                            //$"{callCoverExt}&{callBookId}&{callBookName}&{callBookUrl}&{callBookSize}&{callBookExt}&{callCoverUrl}"
                            if (download_fileInTg == null)
                            {
                                // Faylni yuklab olish uchun URL (manzil)
                                string fileUrl = $"https://e-libraryrest.samdu.uz/{data[6]}";

                                // Faylni yuklab olish uchun HttpClient yaratamiz
                                using (var httpClient = new HttpClient())
                                {
                                    // Faylni olish
                                    var response = await httpClient.GetByteArrayAsync(fileUrl);

                                    // Yangi fayl nomi (misol uchun "new_file_name.pdf")
                                    string newFileName = data[2] + data[0];

                                    // Faylni yuborish uchun InputOnlineFile obyektini yaratamiz
                                    //InputOnlineFile inputFile = new InputOnlineFile(new MemoryStream(response), newFileName);
                                    InputFileStream inputFile = new(new MemoryStream(response), newFileName);

                                    // Faylni yuboramiz
                                    tgMessage = await client.SendDocumentAsync(
                                        chatId: botClientData.chatId,
                                        document: inputFile,
                                        caption: $"<i>Kechirasiz!\nTelegram bot 50MB dan katta fayllarni yubora olmaydi!\nBrowser orqali yuklanshingiz mumkin!</i>\n\n <a href=\"https://e-libraryrest.samdu.uz/{data[3]}\">\"<b>Browser orqali yuklash!</b>\"</a>",
                                        parseMode: ParseMode.Html,
                                        cancellationToken: cancellationToken
                                    );
                                    Console.WriteLine($"Fayl manzili {botClientData.chatId} ga yuborildi! {data[2] + data[5]}");

                                    // Faylni manzilini Id sini saqlash 
                                    string fileAddressId = tgMessage.Document?.FileId;

                                    DownloadedFileAdressToTg fileAdresstoTgBaza = new DownloadedFileAdressToTg
                                    {
                                        fileId = new Guid(data[1]),
                                        fileTgAdressId = fileAddressId
                                    };
                                    context.FileTgAdressDb.Add(fileAdresstoTgBaza);
                                    context.SaveChanges();
                                }
                            }
                            else
                            {
                                //InputOnlineFile inputFile = new InputOnlineFile(download_fileInTg.fileTgAdressId);
                                //InputFileUrl inputFile = new(download_fileInTg.fileTgAdressId);

                                InputFileId inputFile = new(download_fileInTg.fileTgAdressId);



                                // Faylni yuboramiz
                                tgMessage = await client.SendDocumentAsync(
                                        chatId: botClientData.chatId,
                                        //document: inputFile,
                                        document: inputFile,
                                        caption: $"<i>Kechirasiz!\nTelegram bot 50MB dan katta fayllarni yubora olmaydi!\nBrowser orqali yuklanshingiz mumkin!</i>\n\n <a href=\"https://e-libraryrest.samdu.uz/{data[3]}\">\"<b>Browser orqali yuklash!</b>\"</a>",
                                        parseMode: ParseMode.Html,
                                        cancellationToken: cancellationToken
                                    );
                                Console.WriteLine($"Fayl manzili {botClientData.chatId} ga yuborildi! {data[2] + data[5]}");
                            }
                        }
                        else
                        {
                            if (download_fileInTg != null)
                            {
                                try
                                {
                                    //// Faylni foydalanuvchiga yuborish
                                    //InputMediaDocument inputMedia = new InputMediaDocument(new InputMedia(download_fileInTg.fileTgAdress));
                                    //inputMedia.Caption = "Bu dokument izohi";

                                    //await client.SendMediaGroupAsync(
                                    //    chatId: botClientData.chatId,
                                    //    media: new[] { inputMedia },
                                    //    //disableNotification: false
                                    //    //parseMode: ParseMode.Html
                                    //    cancellationToken: cancellationToken
                                    //);


                                    //InputOnlineFile inputFile = new InputOnlineFile(download_fileInTg.fileTgAdressId);
                                    //InputFileUrl inputFile = new (download_fileInTg.fileTgAdressId);
                                    InputFileId inputFile = new(download_fileInTg.fileTgAdressId);


                                    // Faylni yuboramiz
                                    tgMessage = await client.SendDocumentAsync(
                                        chatId: botClientData.chatId,
                                        document: inputFile,
                                        caption: $"{data[2]}\n\n <i>Manba:</i> <a href=\"https://e-library.samdu.uz/\">\"<b>SamDu Elektron kutubxona</b>\"</a>",
                                        parseMode: ParseMode.Html,
                                        cancellationToken: cancellationToken
                                        );

                                    Console.WriteLine($"Fayl {botClientData.chatId} ga yuborildi! {data[2] + data[5]}");
                                }
                                //catch (TelegramRequestException ex)
                                //{
                                //    Console.WriteLine($"Telegram Request xatoligi: {ex.Message}");
                                //}
                                catch (Exception ex)
                                {
                                    Console.WriteLine($"Xatolik yuz berdi: {ex.Message}");
                                }
                            }
                            else
                            {
                                try
                                {

                                    //   0             1             2            3               4             5             6              7
                                    //$"{callCoverExt}&{callBookId}&{callBookName}&{callBookUrl}&{callBookSize}&{callBookExt}&{callCoverUrl}&"

                                    // Faylni yuklab olish uchun URL (manzil)
                                    string fileUrl = $"https://e-libraryrest.samdu.uz/{data[3]}";

                                    // Faylni yuklab olish uchun HttpClient yaratamiz
                                    using (var httpClient = new HttpClient())
                                    {
                                        // Faylni olish
                                        var response = await httpClient.GetByteArrayAsync(fileUrl);

                                        // Yangi fayl nomi (misol uchun "new_file_name.pdf")
                                        string newFileName = data[2] + data[5];

                                        // Faylni yuborish uchun InputOnlineFile obyektini yaratamiz
                                        //InputOnlineFile inputFile = new InputOnlineFile(new MemoryStream(response), newFileName);
                                        InputFileStream inputFile = new(new MemoryStream(response), newFileName);


                                        //Console.WriteLine(inputFile);

                                        // Faylni yuboramiz
                                        tgMessage = await client.SendDocumentAsync(
                                            chatId: botClientData.chatId,
                                            document: inputFile,
                                            caption: $"{data[2]}\n\n <i>Manba:</i> <a href=\"https://e-library.samdu.uz/\">\"<b>SamDu Elektron kutubxona</b>\"</a>",
                                            parseMode: ParseMode.Html,
                                            cancellationToken: cancellationToken
                                        );

                                        Console.WriteLine($"Fayl {botClientData.chatId} ga yuborildi! {data[2] + data[5]}");

                                        // Faylni manzilini Id sini saqlash 
                                        string fileAddressId = tgMessage.Document?.FileId;

                                        DownloadedFileAdressToTg fileAdresstoTgBaza = new DownloadedFileAdressToTg
                                        {
                                            fileId = new Guid(data[1]),
                                            fileTgAdressId = fileAddressId
                                        };
                                        context.FileTgAdressDb.Add(fileAdresstoTgBaza);
                                        context.SaveChanges();

                                    }
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex);
                                }
                            }
                        }

                        await BotStatus6_2(yulanmoqda.MessageId);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                }

                // agarda boshqa kitob yuklash tugamasi bosilmasdan matn yozilsa "Boshqa kitob qidirish" tugmasini bosishni so'rash         8 - 8
                //async Task BotStatus8()
                //{
                //    if (botClientData._inlineBotton_ok)
                //    {
                //        await client.SendTextMessageAsync(
                //            chatId: botClientData.chatId,
                //            text: "Boshqa kitob qidirish uchun! \n\"Boshqa kitob qidirish\" \ntugmasini bosing!",
                //            cancellationToken: cancellationToken
                //        );
                //    }
                //    else
                //    {
                //        await BotStatus5();
                //    }
                //}

                if (botClientData.botStatus == 1)
                {
                    await BotStatus1();
                }
                else if (botClientData.botStatus == 2)// && message.Contact?.PhoneNumber != null && message.Contact.UserId == message.Chat.Id)
                {
                    await BotStatus2();
                }
                else if (botClientData.botStatus == 201)
                {
                    await BotStatus2_1();
                }
                else if (botClientData.botStatus == 3)
                {
                    await BotStatus3();
                }
                else if (botClientData.botStatus == 4)
                {
                    await BotStatus4();
                }
                else if (botClientData.botStatus == 5)
                {
                    //await BotStatus5();
                }
                else if (botClientData.botStatus == 6)
                {
                    await BotStatus6(false, message.Text, 0);
                }
                else if (botClientData.botStatus == 7)
                {

                }
                //else if (botClientData.botStatus == 8)
                //{
                //    await BotStatus8();
                //}

            }

            Task HandlePollingErrorAsync(ITelegramBotClient client, Exception exception, CancellationToken cancellationToken)
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


            string StrTrimAndLengCorect(string? searchBook)
            {
                if (searchBook == null)
                {
                    return "";
                }
                searchBook = searchBook.Trim();

                if (searchBook.Length > 50)
                {
                    return searchBook.Substring(0, 50);
                }
                return searchBook;
            }

            string Num(int? number)
            {
                string numStr = number.ToString();
                int honaSoni = numStr.Length;

                string result = "";
                for (int i = 6; i > honaSoni; i--)
                {
                    result += "0";
                }
                return result + numStr;
            }

            string FileSize(long? size)
            {

                if (size >= 1048576)
                {
                    return (Math.Floor(((float)size) / 10485.76) / 100).ToString() + "MB";
                }
                else if (size >= 1024)
                {
                    return (Math.Floor(((float)size) / 10.24) / 100).ToString() + "KB";
                }
                else
                {
                    return size.ToString() + "Bayt";
                }
            }


            //using (var dbContext = new ApplicationDbContext())
            //{
            //    // Create or query the database
            //    var users = dbContext.Users.ToList();
            //    // Perform other database operations

            //    foreach (var user in users)
            //    {
            //        Console.WriteLine($"User: {user.Username}, Email: {user.Email}");
            //    }
            //}
        }
    }
}
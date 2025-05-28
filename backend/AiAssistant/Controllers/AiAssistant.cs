using System.Text;
using System.Web.Http.Description;
using AiAssistant.Dtos;
using AiAssistant.models;
using AssistanService;
using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace OllamaLLMAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class QueryController : ControllerBase
    {
        private readonly IAssistanceService _assistanceService;
        private readonly ChatContext _context;

        public QueryController(IAssistanceService assistanceService, ChatContext context)
        {
            _assistanceService = assistanceService;
            _context = context;
        }


        [HttpPost("CreateChat")]
        [ResponseType(typeof(ChatDto))]
        public async Task<IActionResult> CreateNewChat()
        {
            Chats newChat = new Chats
            {
                Name = $"Chat {DateTime.Now:yyyy-MM-dd HH:mm:ss}"
            };

            _context.Chats.Add(newChat);
            await _context.SaveChangesAsync();

            return Ok(newChat);
        }

        [HttpGet("AllChats")]
        [ResponseType(typeof(IEnumerable<ChatDto>))]

        public async Task<IActionResult> GetAllChats()
        {
            List<Chats> chats = await _context.Chats.ToListAsync();
            return Ok(chats);
        }


        [HttpGet("MessagesByChat/{chatId}")]
        [ResponseType(typeof(IEnumerable<ChatMessageDto>))]
        public async Task<IActionResult> GetMessagesByChat(int chatId)
        {
            List<DbChatMessages> messages = await _context.Messages
                .Where(m => m.ChatId == chatId)
                .ToListAsync();


            return Ok(messages);
        }


        [HttpPut("RenameChat/{chatId}")]
        [ResponseType(typeof(ChatDto))]
        public async Task<IActionResult> RenameChat(int chatId, [FromBody] RenameChatRequest request)
        {
            Chats? chat = await _context.Chats.FindAsync(chatId);
            if (chat == null)
            {
                return NotFound($"Chat with ID {chatId} not found.");
            }

            chat.Name = request.NewName;
            await _context.SaveChangesAsync();

            return Ok(chat);
        }
        public class RenameChatRequest
        {
            public string NewName { get; set; } = string.Empty;
        }



        [HttpDelete("DeleteChat/{chatId}")]
        [ResponseType(typeof(ChatDto))]
        public async Task<IActionResult> DeleteChat(int chatId)
        {
            Chats? chat = await _context.Chats
                .Include(c => c.Messages) 
                .FirstOrDefaultAsync(c => c.Id == chatId);

            if (chat == null)
                return NotFound($"Chat with ID {chatId} not found.");

            IQueryable<DbChatMessages> messages = _context.Messages.Where(m => m.ChatId == chatId);
            _context.Messages.RemoveRange(messages);

            _context.Chats.Remove(chat);
            await _context.SaveChangesAsync();

            return Ok($"Chat {chatId} and its messages have been deleted.");
        }


        [HttpGet]
        [ResponseType(typeof(IEnumerable<ChatMessageDto>))]
        public async Task<IActionResult> Get(string input, int chatId, string currentDateTime)
        {
            Chats? chat = await _context.Chats.FindAsync(chatId);
            if (chat == null)
            {
                return NotFound($"Chat with ID {chatId} does not exist.");
            }

            List<DbChatMessages> messages = await _context.Messages
                .Where(m => m.ChatId == chatId)
                .OrderBy(m => m.Id)
                .ToListAsync();

            List<(string Role, string Text)> conversationHistory = new List<(string Role, string Text)>();

            string informations = "[\n" +
            "  { \"NazwaKlasy\": \"1TI\", \"IloscOsob\": \"28\", \"Profil\": \"Technik informatyk\", \"SredniaEgzaminZawodowy\": \"Brak danych\", \"SredniaMatura\": \"Brak danych\", \"Miasto\": \"Warszawa\", \"RokRozpoczecia\": \"2024\", \"LiczbaPraktykWTygodniu\": \"1\", \"DostepDoNowoczesnegoSprzetu\": true, \"UdzialWProjektachUE\": true },\n" +
            "  { \"NazwaKlasy\": \"1TL\", \"IloscOsob\": \"26\", \"Profil\": \"Technik logistyk\", \"SredniaEgzaminZawodowy\": \"Brak danych\", \"SredniaMatura\": \"Brak danych\", \"Miasto\": \"Kraków\", \"RokRozpoczecia\": \"2024\", \"LiczbaPraktykWTygodniu\": \"1\", \"DostepDoNowoczesnegoSprzetu\": true, \"UdzialWProjektachUE\": false },\n" +
            "  { \"NazwaKlasy\": \"2TM\", \"IloscOsob\": \"27\", \"Profil\": \"Technik mechanik\", \"SredniaEgzaminZawodowy\": \"62.3\", \"SredniaMatura\": \"58.9\", \"Miasto\": \"Poznań\", \"RokRozpoczecia\": \"2023\", \"LiczbaPraktykWTygodniu\": \"2\", \"DostepDoNowoczesnegoSprzetu\": false, \"UdzialWProjektachUE\": false },\n" +
            "  { \"NazwaKlasy\": \"2TE\", \"IloscOsob\": \"25\", \"Profil\": \"Technik elektryk\", \"SredniaEgzaminZawodowy\": \"67.4\", \"SredniaMatura\": \"61.2\", \"Miasto\": \"Gdańsk\", \"RokRozpoczecia\": \"2023\", \"LiczbaPraktykWTygodniu\": \"2\", \"DostepDoNowoczesnegoSprzetu\": true, \"UdzialWProjektachUE\": true },\n" +
            "  { \"NazwaKlasy\": \"3TG\", \"IloscOsob\": \"30\", \"Profil\": \"Technik grafiki i poligrafii cyfrowej\", \"SredniaEgzaminZawodowy\": \"73.8\", \"SredniaMatura\": \"64.5\", \"Miasto\": \"Wrocław\", \"RokRozpoczecia\": \"2022\", \"LiczbaPraktykWTygodniu\": \"1\", \"DostepDoNowoczesnegoSprzetu\": true, \"UdzialWProjektachUE\": true },\n" +
            "  { \"NazwaKlasy\": \"3TH\", \"IloscOsob\": \"24\", \"Profil\": \"Technik hotelarstwa\", \"SredniaEgzaminZawodowy\": \"69.0\", \"SredniaMatura\": \"59.8\", \"Miasto\": \"Lublin\", \"RokRozpoczecia\": \"2022\", \"LiczbaPraktykWTygodniu\": \"2\", \"DostepDoNowoczesnegoSprzetu\": false, \"UdzialWProjektachUE\": false },\n" +
            "  { \"NazwaKlasy\": \"4TI\", \"IloscOsob\": \"29\", \"Profil\": \"Technik informatyk\", \"SredniaEgzaminZawodowy\": \"78.6\", \"SredniaMatura\": \"70.1\", \"Miasto\": \"Warszawa\", \"RokRozpoczecia\": \"2021\", \"LiczbaPraktykWTygodniu\": \"2\", \"DostepDoNowoczesnegoSprzetu\": true, \"UdzialWProjektachUE\": true },\n" +
            "  { \"NazwaKlasy\": \"4TZ\", \"IloscOsob\": \"23\", \"Profil\": \"Technik żywienia i usług gastronomicznych\", \"SredniaEgzaminZawodowy\": \"66.7\", \"SredniaMatura\": \"62.0\", \"Miasto\": \"Katowice\", \"RokRozpoczecia\": \"2021\", \"LiczbaPraktykWTygodniu\": \"1\", \"DostepDoNowoczesnegoSprzetu\": false, \"UdzialWProjektachUE\": true },\n" +
            "  { \"NazwaKlasy\": \"5TA\", \"IloscOsob\": \"22\", \"Profil\": \"Technik architektury krajobrazu\", \"SredniaEgzaminZawodowy\": \"71.2\", \"SredniaMatura\": \"65.4\", \"Miasto\": \"Rzeszów\", \"RokRozpoczecia\": \"2020\", \"LiczbaPraktykWTygodniu\": \"2\", \"DostepDoNowoczesnegoSprzetu\": true, \"UdzialWProjektachUE\": false },\n" +
            "  { \"NazwaKlasy\": \"5TL\", \"IloscOsob\": \"27\", \"Profil\": \"Technik logistyk\", \"SredniaEgzaminZawodowy\": \"75.9\", \"SredniaMatura\": \"68.3\", \"Miasto\": \"Szczecin\", \"RokRozpoczecia\": \"2020\", \"LiczbaPraktykWTygodniu\": \"2\", \"DostepDoNowoczesnegoSprzetu\": true, \"UdzialWProjektachUE\": true }\n" +
            "]";

            conversationHistory.Add(("Assistant", $"You are a helpful school guide. Use the following information to assist the user:\n{informations}"));

            foreach (DbChatMessages msg in messages)
            {
                conversationHistory.Add((msg.Role.ToString(), msg.Text));
            }

            conversationHistory.Add(("User", input));

            StringBuilder promptBuilder = new StringBuilder();
            foreach ((string Role, string Text) in conversationHistory)
            {
                promptBuilder.AppendLine($"{Role}: {Text}");
            }

            string prompt = promptBuilder.ToString();

            string response = await _assistanceService.AskBot(prompt);

            _context.Messages.Add(new DbChatMessages
            {
                ChatId = chatId,
                Date = currentDateTime,
                Role = ChatRole.User,
                Text = input
            });

            _context.Messages.Add(new DbChatMessages
            {
                ChatId = chatId,
                Date = currentDateTime,
                Role = ChatRole.Assistant,
                Text = response
            });

            await _context.SaveChangesAsync();

            return Ok(response);
        }


    }
}

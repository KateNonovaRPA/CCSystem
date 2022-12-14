namespace Models
{
    public class MessageConstants
    {
        public const string SystemTitle = "";
        public const string SavedOK = "Записът премина успешно.";
        public const string SavedFailed = "Проблем по времена запис. Моля, опитайте по-късно.";

        public const string RegisterMail_Subject = "Регистрация на потребител";
        public const string RegisterMail_Body = "Здравейте {0},<br/> Вашата регистрация в <b>{1}</b> е успешна. За да можете да използвате пълната функционалност на системата, моля потвърдете вашата регистрация като последвате <a href='{2}'>следния линк</a>";

        public const string RegisterOK_Title = "Поздравления!";
        public const string RegisterOK_Message = "Успешно потвърдихте регистрацията си.";
        public const string RegisterFailed_Message = "Проблем по време на потвърждение на потребител.";


        public const string ConfirmMail_NotFound = "Невалиден или ненамерен потребител";

        public const string ProfileOK_Title = "";
        public const string ProfileOK_Message = "Данните за вашия потребител са актуализирани успешно!";

        public const string ChangePassword_Subject = "Забравена парола";

        public const string ChangePassword_Message = "";

    public const string ContactUsMail_Subject = "Съобщение от потребител";
    public const string ContactUsMail_Body = "Здравейте Администратор,<br/> Получавате това съобщение от <b>{0}</b> с електронна поща <b>{1}</b>.<br><br> <i>Относно:</i> <b>{2}</b> <i>Текст на съобщението:</i><br><br>{3}<br><br>";
  }
}

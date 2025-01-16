namespace UserService.BLL.Services
{
    public class EmailTemplateLoader
    {
        private readonly string _templateDirectory;

        public EmailTemplateLoader(string templateDirectory)
        {
            _templateDirectory = templateDirectory;
        }

        public string LoadTemplate(string templateName)
        {
            var filePath = Path.Combine(_templateDirectory, templateName);
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"Email template '{templateName}' not found at '{filePath}'");
            }

            return File.ReadAllText(filePath);
        }
    }
}

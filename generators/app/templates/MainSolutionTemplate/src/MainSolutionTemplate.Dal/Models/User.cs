namespace MainSolutionTemplate.Dal.Models
{
  public class User : BaseDalModelWithId
  {
    public string Email { get; set; }
    public string PasswordAndSeed { get; set; }
    public string DisplayName { get; set; }
  }
}
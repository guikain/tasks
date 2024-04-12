namespace Models.Todo;
class Todo{
  public Guid Id {get; set;}
  public string Nome {get; set;}
  public Boolean IsComplete {get; set;}

    public Todo(string nome, Boolean isComplete)
    {
        this.Id = Guid.NewGuid();
        this.Nome = nome;
        this.IsComplete = isComplete;
    }
}
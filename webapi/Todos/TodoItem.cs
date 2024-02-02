// create model class for a todo list item
using System.ComponentModel;
using Swashbuckle.AspNetCore.Annotations;

public class TodoItem
{
    //don't allow the id to be set by the user
    [ReadOnly(true)]
    [SwaggerSchema(ReadOnly = true)]
    public long? Id { get; set; }
    public string? Title { get; set; }
    //set the default value of IsCompleted to false
    [DefaultValue(false)]
    public bool IsCompleted { get; set; }
}
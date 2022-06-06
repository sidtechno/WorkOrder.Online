namespace WorkOrder.Online.Models
{
    public class TaskListViewModel : BaseViewModel
    {
        public IEnumerable<TaskViewModel> Tasks { get; set; }
    }
}

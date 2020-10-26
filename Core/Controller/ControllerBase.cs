using Microsoft.EntityFrameworkCore;

namespace socket_sharp.Core.Controller
{
    public abstract class ControllerBase
    {
        public DbContext DbContext { get; set; }
    }
}
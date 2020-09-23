using System.Threading;
using System.Threading.Tasks;
using SoftDeletes.Core;

namespace socket_sharp.Extensions
{
    public class ModelExtension : SoftDeletes.ModelTools.ModelExtension
    {
        public long Id { get; set; }

        public override Task OnSoftDeleteAsync(DbContext context, CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }

        public override void OnSoftDelete(DbContext context)
        {
        }

        public override Task LoadRelationsAsync(DbContext context, CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }

        public override void LoadRelations(DbContext context)
        {
        }
    }
}
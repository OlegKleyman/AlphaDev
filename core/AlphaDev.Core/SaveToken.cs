using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlphaDev.Core
{
    public class SaveToken : ISaveToken
    {
        private readonly SaveAction _save;

        public SaveToken(SaveAction save) => _save = save;

        public async Task SaveAsync()
        {
            await _save();
        }
    }
}
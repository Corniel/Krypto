using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace GentleWare.Krypto.Web.Controllers
{
	public class KryptoController : ApiController
	{
		public IEnumerable<string> Get(string query)
		{
			return KryptoSolver.Solve(query).Select(node => node.ToString());
		}
	}
}
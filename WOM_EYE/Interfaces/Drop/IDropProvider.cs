using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WOM_EYE.Models.Drop;

namespace WOM_EYE.Interfaces.Drop
{
	public	interface IDropProvider
	{
		public List<DropModel> getAllDrop();

		public ResponseMessageDrop DropProject(DropModel form);

		public ResponseMessageDrop UnDropProject(String dropId);


	}
}

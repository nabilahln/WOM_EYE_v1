using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WOM_EYE.Models.Projects;

namespace WOM_EYE.Interfaces.Projects
{
	public interface IDetailProjectProvider
	{
		List<DetailProjectModel> getAllDetailProject(int id);
		DetailProjectModel getDetailProjectById(int id);
		ResponseMessage UpdateDetailProject(DetailProjectModel form);

	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WOM_EYE.Models.Projects;

namespace WOM_EYE.Interfaces.Projects
{
	public interface IProjectProvider
	{
		List<ProjectModel> getAllProject();
		ProjectModel getDataProjectById(int id);
		ProjectModel getDataProjectByIdEdit(int id);
		List<SelectListUser> ddlUser();
		List<SelectListStatus> ddlStatus();
		List<SelectListJenis> ddlJenis();

		ResponseMessage InsertProject(ProjectModel form);
		ResponseMessage UpdateProject(ProjectModel form);
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WOM_EYE.Models.Notes;

namespace WOM_EYE.Interfaces.Notes
{
	public interface INoteProvider
	{
		public List<NoteModel> getAllCatatan(int detailProjectId);
		public ResponseMessageCatatan InsertCatatan(NoteModel form);
		List<SelectListStatusCatatan> ddlStatusCatatan();
		public NoteModel getDataCatatanById(int id);
		public ResponseMessageCatatan UpdateCatatan(NoteModel form);
	}
}

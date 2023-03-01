using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WOM_EYE.Models.Progress;

namespace WOM_EYE.Interfaces.Progress
{
	public interface IProgressProvider
	{
		List<ProgressModel> getAllProgress(int id);

		ResponseMessage InsertDataProgress(ProgressModel form);

		ResponseMessage UpdateProgress(ProgressModel form);

		ProgressModel getDataProgressById(int id);

		ResponseMessage DeleteProgress(int id);

	}
}

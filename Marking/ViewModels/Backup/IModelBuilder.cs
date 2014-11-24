using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Marking.ViewModels
{
    public interface IModelBuilder<TViewModel, TEntity>
    {
        TViewModel CreateFrom(TEntity entity);
        TViewModel Rebuild(TViewModel model);
        TEntity ApplyChanges(TViewModel viewModel);
    }
}
﻿using System.Collections.Generic;
using MarkDoc.Core;
using ReactiveUI;

namespace MarkDoc.MVVM.Helpers
{
  public abstract class BaseViewModel
    : ReactiveObject, IViewModel
  {
    /// <inheritdoc />
    public virtual void SetNamedArguments(IReadOnlyDictionary<string, string> arguments) {}
  }
}
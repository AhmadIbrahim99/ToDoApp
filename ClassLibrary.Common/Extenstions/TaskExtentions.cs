﻿using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace WebApplication5.Extenstions
{
    public static class TaskExtentions
    {
        public static ConfiguredTaskAwaitable AnyContext(this Task task)=> task.ConfigureAwait(false);  
        public static ConfiguredTaskAwaitable<T> AnyContext<T>(this Task<T> task)=> task.ConfigureAwait(false);
    }
}

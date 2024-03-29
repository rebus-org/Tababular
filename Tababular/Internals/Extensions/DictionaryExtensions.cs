﻿using System;
using System.Collections.Generic;

namespace Tababular.Internals.Extensions;

static class DictionaryExtensions
{
    public static TValue GetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, Func<TKey, TValue> newValueFactory)
    {
        if (dictionary.TryGetValue(key, out var existing)) return existing;

        var newValue = newValueFactory(key);

        dictionary[key] = newValue;

        return newValue;
    }
}
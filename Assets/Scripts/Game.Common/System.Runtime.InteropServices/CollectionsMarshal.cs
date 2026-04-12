using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace System.Runtime.InteropServices;

public static class CollectionsMarshal
{
    public static Span<T> AsSpan<T>(List<T> list)
    {
        if (list == null)
            return default;
        return Unsafe.As<StrongBox<T[]>>(list).Value.AsSpan(0, list.Count);
    }
}
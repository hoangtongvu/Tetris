namespace EnumGenerators;

public readonly record struct EnumRequestInfo(
    string EnumNamespace,
    string EnumName,
    string MemberName,
    int ExplicitMemeberValue
);
namespace DauTay.Common;

public static class StaticMessageResponse
{
    public static readonly string ERROR = "error";
    public static readonly string SUCCESS = "success";
    public static readonly string UNAUTHORIZED = "unauthorized";
    public static readonly string FORBIDDEN = "forbidden";
    public static readonly string PARAM_INVALID = "param_invalid"; // wrong format or missing
    public static readonly string PARAM_INVALID_VALUE = "param_invalid_value"; // right format, but wrong value
    public static readonly string TOKEN_EXPIRED = "token_expired";
    public static readonly string TOKEN_INVALID = "token_invalid";
    public static readonly string NO_CONTENT = "no_content";
    public static readonly string DUPLICATED = "duplicated";
    public static readonly string INVALID_STATE = "invalid_state"; // request params is correct + ser

    public static readonly string CAN_NOT_INSERT = "can_not_insert";
    public static readonly string CAN_NOT_UPDATE = "can_not_update";
    public static readonly string CAN_NOT_DELETE = "can_not_delete";
}

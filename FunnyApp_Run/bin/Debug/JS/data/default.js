
var disk="D:";

//消息和发送计数器
function C_Msg(ID,Msg){
    this.ID=ID;
    this.Msg=Msg;
    this.Count=0;
}


function get_token(){
    var url="http://www.funnyai.com/login_get_token_json.php";
    var name=s_file.Ini_Read(disk+"\\Net\\Web\\main.ini","main","account");
    var md5=s_file.Ini_Read(disk+"\\Net\\Web\\main.ini","main","md5");
    var data="email="+s_string.urlencode(name)+"&password="+s_string.urlencode(md5);
    var result=s_net.http_post(url,data);
    var token="";
    if (result.indexOf("登录成功")>-1){
        var strSplit=result.split("=");
        token=strSplit[2];
    }
    return token;
}
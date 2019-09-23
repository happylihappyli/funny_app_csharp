
function New_URL(data){
    s_ui.Run_App("chrome.exe", data);
}

s_ui.Web_Init("web",10,30,600,390);

s_ui.Web_New_Event("web","New_URL");




var a=s_net.http_get("http://www.ckxxbao.com/");


var dt=new Date();
var year=dt.getFullYear();
var month=dt.getMonth()+1;
var day=dt.getDate();
if (month<10) month="0"+month;
if (day<10) day="0"+day;

var str='<a href="http://www.ckxxbao.com/cankaoxiaoxidianziban/(.*?)\\.html" target="_blank">参考消息电子版在线阅读 '+year+month+day+'</a>';

//s_ui.text_set("txt_info",str);
//s_ui.msg(a);

var x=new RegExp(str,'g').exec(a);
if (x==null){
    //s_ui.text_set("txt_info","null");
    s_ui.ShowInTask(0);
    sys.Exit();
}else{
    s_ui.Form_Title("抓取参考消息");
    s_ui.Show_Form(700,500);
    //s_ui.ShowInTask(0);
    var html=x[0];
    //s_ui.msg();
    s_ui.Web_Content("web",html);
}



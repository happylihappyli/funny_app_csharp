
function New_URL(data){
    sys.Run_App("chrome.exe", data);
}

sys.Web_Init("web",10,30,600,390);

sys.Web_New_Event("web","New_URL");




var a=sys.Net_Http_GET("http://www.ckxxbao.com/");


var dt=new Date();
var year=dt.getFullYear();
var month=dt.getMonth()+1;
var day=dt.getDate();
if (month<10) month="0"+month;
if (day<10) day="0"+day;

var str='<a href="http://www.ckxxbao.com/cankaoxiaoxidianziban/(.*?)\\.html" target="_blank">参考消息电子版在线阅读 '+year+month+day+'</a>';

//sys.Text_Set("txt_info",str);
//sys.Msg(a);

var x=new RegExp(str,'g').exec(a);
if (x==null){
    //sys.Text_Set("txt_info","null");
    sys.ShowInTask(0);
    sys.Exit();
}else{
    sys.Form_Title("抓取参考消息");
    sys.Show_Form(700,500);
    //sys.ShowInTask(0);
    var html=x[0];
    //sys.Msg();
    sys.Web_Content("web",html);
}




function event_chat(data){
    var obj=JSON.parse(data);
    var friend=obj.from;
    
    if (obj.type=="encrypt"){
    }else{
        var index=s_ui.listbox_select("list_friend",friend);
        if (index==-1){
            //s_ui.msg("no_friend:"+friend);
            s_ui.Notification("no_friend:",friend);
            friend_list("");
            //s_ui.listbox_select("list_friend",friend);
        }
        var strMsg=s_time.Time_Now()+"<br>"+obj.message;
        s_file.append(disk+"\\Net\\Web\\log\\"+friend+".txt",s_time.Date_Now()+" "+strMsg+"\r\n");
        
        var msg=obj.message;
        var r;
        msg=head+"\n"+msg;
        
        log_msg=s_time.Time_Now()+" "+friend+" &gt; <span style='color:#aaaaaa;'>"+obj.to+"</span>"
        +"<br><div>"+msg+"</div><br><br>\r\n"+log_msg;
        
        var id=obj.id;
        var strLine="{\"from\":\""+userName+"\",\"type\":\"chat_return\",\"to\":\""+obj.from+"\",\"oid\":\""+id+"\",\"message\":\""+id+"\"}";
        //s_net.Send_Msg("sys_event",strLine); //消息返回
        
    }
    
    s_ui.Web_Content("web",css_head+log_msg);
}
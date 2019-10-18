
function event_chat(data){
    var obj=JSON.parse(data);
    var friend=obj.from;
    
    if (obj.type=="encrypt"){
    }else{
        var index=s_ui.listbox_select("list_friend",friend);
        if (index==-1){
            s_ui.msg("no_friend:"+friend);
            //friend_list("");
            //s_ui.listbox_select("list_friend",friend);
        }
        var strMsg=s_time.Time_Now()+"<br>"+obj.message;
        s_file.append(disk+"\\Net\\Web\\log\\"+friend+".txt",s_time.Date_Now()+" "+strMsg+"\r\n");
        
        var msg=obj.message;
        var r;
        switch(sep){
            case "file_list"://file
                //head="<tr><th>权限</th><th>子目录(文件)数</th><th>所属用户</th><th>所属用户组</th><th>大小</th><th colspan=3>最近修改/查看时间</th><th>名称</th></tr>";
                var strSplit=msg.split('\n');
                msg="";
                for (var i=0;i<strSplit.length;i++){
                    r = /\s+/g;
                    strSplit[i] = strSplit[i].replace(r, '|');
                    var strSplit2=strSplit[i].split('|');
                    
                    msg+="<tr>";
                    for (var j=0;j<strSplit2.length;j++){
                        var value=strSplit2[j];
                        if (j==8){
                            msg+="<td>"+value+"</td><td>"
                            +"<a href='http://file.edit?file="+value+"' target=_blank>编辑</a></td>";
                        }else{
                            msg+="<td>"+value+"</td>";
                        }
                    }
                    msg+="</tr>";
                }
                break;
            case 1:
                r = /\n/g;
                msg = msg.replace(r, '</td></tr><tr><td>');
                r = /\s+/g;
                msg = msg.replace(r, '</td><td>');
                break;
            case 2:
                r = /\n/g;
                msg = msg.replace(r, '</td></tr><tr><td>');
                r = /:/g;
                msg = msg.replace(r, '</td><td>');
                break;
        }
        msg=head+"\n"+msg;
        
        log_msg=s_time.Time_Now()+" "+friend+" &gt; <span style='color:#aaaaaa;'>"+obj.to+"</span>"
        +"<br><div><table id=data>"
        +msg+"</table></div><br><br>\r\n"+log_msg;
        
        var id=obj.id;
        var strLine="{\"from\":\""+userName+"\",\"type\":\"chat_return\",\"to\":\""+obj.from+"\",\"oid\":\""+id+"\",\"message\":\""+id+"\"}";
        //s_net.Send_Msg("sys_event",strLine); //消息返回
        
    }
    
    s_ui.Web_Content("web",css_head+log_msg);
}
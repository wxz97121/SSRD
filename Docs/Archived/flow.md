# 战斗流程

输入一个小节，演出结算一个小节，有留白的小节

前导小节‖:输入小节|演出小节:‖

            
## 【前导小节】

|xxxxxxxx xxxxxxxx|  
进战斗、转阶段的时候转场动画用，可以随便按键，出音效不算判定（练手）

## 【输入小节】

xx|o-o-o-o- o-o-oxxx|  
用户输入指令的阶段，实时展示输入信息，也是双方博弈的核心阶段

1. 开始输入判定：从上一小节的末尾，本小节第一个判定位之前的t时间内就开始接受判定

2. 接受输入中... 直到

> 某次输入被判定为错误  
 在最后一个“o”正确输入  
 最后一个“o”的判定时间结束

3. 结束输入判定

    此时已经决定了本回合的结果

4. 平稳度过本小节剩余的时间

## 【演出小节】

|xxxxfxxx dxxxjxxx|
在此按照上一个【输入小节】的结果进行演出，角色的属性也在演出时变化

1. 本小节开始就播放角色的行动动画的预备动画：“人物攒动画-预备动作”、“人物防动画-预备动作”、“人物波动画-预备动作”、“人物必杀动画-预备动作”

2. 到达f点时，统一支付资源
> 主要是【波】和【必杀技】需要支付的灵力或其他资源，播放至f时，UI才进行变化。人物动画在这一拍有一个聚集能量完毕的动作交代：“人物攒动画-前摇动作”、“人物波动画-聚气动作”、“人物防动画-前摇动作”、“人物必杀动画-爆气动作”  

> 如果此时有人死亡，死亡的角色变为播放“人物死亡动画”

3. 到达d点时，是攻击互相怼的瞬间

> 人物动画进入发出阶段：“人物攒动画-马上攒好了”、“人物波动画-波放到屏幕中间”、“人物防动画-防的姿势摆好了”、“人物必杀动画-招式防到一半”  
>【攻击】是否怼到一起出现了【相杀】，有的话播放“相杀特效”   
> “人物防御动画”动画在此刻完成，如果遇到攻击则播放“防御特效”（防住没防住后面展示）

4. 到达j点时，统一结算最终结果
> 普通情况下人物动画放完：“人物攒动画-攒到了！”、“人物波动画-波已屏幕贯穿！”、“人物防动画-继续摆好姿势”、“人物必杀动画-招式完全放出”  
> 如果相杀平局继续播放”相杀平局特效“（是“相杀特效”的升级版），同时双方播放的内容改为“人物波动画-对波了！”、“人物必杀动画-必杀被怼了”  
> 是否防御成功，播放”防御特效“，防御成功不影响对方的人物攻击动画  
> 是否造成【伤害】,有伤害则从原动画改为播放“人物挨打动画”     
> 相应属性的UI在此时进行变更

>如果此时有人死亡，死亡的角色变为播放“人物死亡动画”  

5. 演出小节结束，进入下一个输入小节

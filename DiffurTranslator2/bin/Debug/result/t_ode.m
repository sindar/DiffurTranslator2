function t_ode()
tspan=[1:1:100]
x0=[620;10;70];
[t,x]=ode45(@t_funsys,tspan,x0);
f = figure('Visible','off')
plot (t,x(:,[1,2,3]),'lineWidth',3);
grid on
legend('x`1','x`2','x`3')
print('-dbmp','-r80','graf_ode.bmp')


function t_ode()
tspan=[0:0.6:50]
x0=[620.0;10.1;70.1];
[t,x]=ode45(@t_funsys,tspan,x0);
f = figure('Visible','off')
plot (t,x(:,[1,2,3]),'lineWidth',3);
grid on
legend('x`1','x`2','x`3')
print('-dbmp','-r80','graf_ode.bmp')


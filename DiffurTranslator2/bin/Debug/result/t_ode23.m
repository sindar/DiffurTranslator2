function t_ode()
tspan=[0:0.5:20]
x0=[100;0;0];
[t,x]=ode23(@t_funsys,tspan,x0);
f = figure('Visible','off')
plot (t,x(:,[1,2,3]),'lineWidth',3);
grid on
legend('x`1','x`2','x`3')
print('-dbmp','-r80','graf_ode23.bmp')


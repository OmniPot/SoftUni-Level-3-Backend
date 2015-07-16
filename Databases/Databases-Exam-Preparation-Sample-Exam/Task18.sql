select
	p.name as product_name,
	count(o.id) as num_orders,
	ifnull(sum(oi.quantity), 0.00) as quantity,
	truncate(p.price, 2) as price,
	ifnull(truncate(p.price * sum(oi.quantity), 4), 0.0000) as total_price
from `products` as p
left join `order_items` as oi
	on p.id = oi.productId
left join `orders` as o
	on o.id = oi.orderId
group by p.name
order by product_name
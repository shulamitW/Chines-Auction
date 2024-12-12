import React from 'react';
import { TabMenu } from 'primereact/tabmenu';
import { useNavigate } from 'react-router-dom';
import { Badge } from 'primereact/badge';

export default function Home(props) {
    const { activeIndex, onTabChange } = props;
    const [localActiveIndex, setLocalActiveIndex] = React.useState(activeIndex || 0);
    const navigate = useNavigate();
    const role = localStorage.getItem('role');
  
    const items = [
      { label: 'Gifts', icon: 'pi pi-gift', url: '/PresentsManagement' },
      { label: 'Purchase', icon: 'pi pi-shopping-cart', url: '/PurchasesManagement' },
      { label: 'Donors', icon: 'pi pi-users', url: '/Donors' }, // Changed icon
      //{ label: 'Cart', icon: 'pi pi-shopping-cart', badge: props.cartCount, template: itemRenderer  },
      //{ label: 'Presents', icon: 'pi pi-gift', url: '/Presents' }, // Changed icon
      { label: 'Sign Out', icon: 'pi pi-sign-out', command: () => { GoOut() } }
    ];

      if (role !== 'Manager') {
        items.splice(2, 0, { label: 'Donors', icon: 'pi pi-users', url: '/donor' });
        items.splice(4, 0, { label: 'Purchasing_management', icon: 'pi pi-file', url: '/Purchasing_management' });
    }

    const itemRenderer = (item) => (
        <a className="flex align-items-center p-menuitem-link" onClick={(props.openCart)}>
            <span className={item.icon} />
            <span className="mx-2">{item.label}</span>
            {item.badge && <Badge className="ml-auto" value={item.badge} />}
            {item.shortcut && <span className="ml-auto border-1 surface-border border-round surface-100 text-xs p-1">{item.shortcut}</span>}
        </a>
    );



  const onLocalTabChange = (e) => {
    setLocalActiveIndex(e.index); // Update local active index on tab change
    if (onTabChange) {
      onTabChange(e.index); // Call the provided onTabChange function if available
    }
    navigate(items[e.index].url); // Navigate to the selected tab's URL
  };

  const GoOut = () => {
    console.log('Logging out...');
    localStorage.removeItem('loggedInUser');
    localStorage.removeItem('token'); // הסרת ה-token אם קיים
    localStorage.removeItem('role'); // הסרת ה-token אם קיים
    navigate('/', { replace: true }); // משתמשים ב-replace כדי לוודא שדף הבית ייטען מחדש
    navigate(0)
    console.log('Navigating to home...');
  };

  return (
    <div className="card d-flex flex-row justify-content-between align-items-center">
      <TabMenu model={items} activeIndex={localActiveIndex} onTabChange={onLocalTabChange} />
    </div>
  );
}

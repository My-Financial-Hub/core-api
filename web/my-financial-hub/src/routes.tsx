import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';

import { ApisContextProvider } from './commom/contexts/api-context';

import App from './pages/App';
import NotFoundPage from './pages/not-found';
import AccountsPage from './pages/accounts';
import CategoriesPage from './pages/categories';
import TransactionsPage from './pages/transactions';

//TODO: improve context logic 
export default function AppRouter() {
  return (
    <Router>
      {/* <Routes>
        <Route path='/' element={<App />} />   
        <Route path='*' element={<NotFoundPage />} />
      </Routes > */}
      <ApisContextProvider>
        <Routes>
          <Route path='/' element={<App />} />   
          <Route path='/accounts' element={<AccountsPage />} />
          <Route path='/categories' element={<CategoriesPage />} />
          <Route path='/transactions' element={<TransactionsPage />} />
          <Route path='*' element={<NotFoundPage />} />
        </Routes>
      </ApisContextProvider>
    </Router >
  );
}
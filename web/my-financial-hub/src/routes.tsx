import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';

import { ApisContextProvider } from './commom/contexts/api-context';

import App from './pages/App';
import NotFoundPage from './pages/not-found';
import AccountsPage from './pages/accounts';

//TODO: improve context logic
export default function AppRouter() {
  return (
    <Router>
      <ApisContextProvider>
        <Routes>
          <Route path='/' element={<App />} />
          <Route path='*' element={<NotFoundPage />} />
          <Route path='/accounts' element={<AccountsPage />} />
        </Routes>
      </ApisContextProvider>
    </Router>
  );
}